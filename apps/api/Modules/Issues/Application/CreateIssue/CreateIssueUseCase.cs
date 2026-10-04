using FluentValidation;
using FixFlow.Api.Common.Application;
using FixFlow.Api.Common.Domain;
using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;
using FixFlow.Api.Modules.Issues.Domain;
using FixFlow.Contracts.Events.Issues;
using MassTransit;

namespace FixFlow.Api.Modules.Issues.Application.CreateIssue;

public record CreateIssueCommand(
    string Title,
    string RawDescription,
    string Location
) : ICommand<Result<CreateIssueResponse>>;

public record CreateIssueResponse(
    Guid IssueId,
    string Title,
    string Status,
    DateTime CreatedAtUtc
);

public class CreateIssueValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.RawDescription)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters for proper analysis.")
            .MaximumLength(4000).WithMessage("Description must not exceed 4000 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");
    }
}

public class CreateIssueHandler(
    ICurrentUserProvider currentUser,
    IPublishEndpoint publishEndpoint,
    ILogger<CreateIssueHandler> logger)
    : ICommandHandler<CreateIssueCommand, Result<CreateIssueResponse>>
{
    public async Task<Result<CreateIssueResponse>> HandleAsync(CreateIssueCommand command, CancellationToken cancellationToken = default)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == null)
        {
            throw new ForbiddenException("Authentication required to report issues.");
        }

        var issueId = Guid.NewGuid();
        var issue = new Issue(
            issueId,
            currentUser.UserId.Value,
            command.Title,
            command.RawDescription,
            command.Location
        );

        issue.MarkAnalysisRequested();

        // Publish event to RabbitMQ for AI worker processing
        var correlationId = Guid.NewGuid().ToString();
        await publishEndpoint.Publish(new IssueAnalysisRequested(
            issue.Id,
            issue.ReporterId,
            issue.RawDescription,
            issue.Location,
            Guid.NewGuid(),
            DateTime.UtcNow,
            correlationId
        ), cancellationToken);

        logger.LogInformation("Issue {IssueId} created by user {UserId}. Dispatched analysis request with CorrelationId {CorrelationId}",
            issue.Id, currentUser.UserId, correlationId);

        return Result<CreateIssueResponse>.Success(new CreateIssueResponse(
            issue.Id,
            issue.Title,
            issue.Status.ToString(),
            issue.CreatedAtUtc
        ));
    }
}
