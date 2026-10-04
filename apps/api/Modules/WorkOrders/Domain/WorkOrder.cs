using FixFlow.Api.Common.Domain;
using FixFlow.Api.Common.Exceptions;

namespace FixFlow.Api.Modules.WorkOrders.Domain;

public enum WorkOrderStatus
{
    AwaitingResponse = 1,
    Accepted = 2,
    Rejected = 3,
    Expired = 4,
    Cancelled = 5,
    ReadyToStart = 6,
    InProgress = 7,
    TechnicianCompleted = 8,
    AwaitingReporter = 9,
    Completed = 10,
    Reopened = 11
}

public class WorkOrder : Entity<Guid>, IAggregateRoot
{
    public Guid IssueId { get; private set; }
    public Guid ReporterId { get; private set; }
    public Guid AssignedTechnicianId { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public DateTime AwaitingResponseUntilUtc { get; private set; }
    public DateTime? AcceptedAtUtc { get; private set; }
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? TechnicianCompletedAtUtc { get; private set; }
    public DateTime? ClosedAtUtc { get; private set; }
    public string? RejectionReason { get; private set; }
    public string? CompletionNotes { get; private set; }

    private WorkOrder() { }

    public WorkOrder(Guid id, Guid issueId, Guid reporterId, Guid assignedTechnicianId, TimeSpan responseWindow)
        : base(id)
    {
        IssueId = issueId;
        ReporterId = reporterId;
        AssignedTechnicianId = assignedTechnicianId;
        Status = WorkOrderStatus.AwaitingResponse;
        AwaitingResponseUntilUtc = DateTime.UtcNow.Add(responseWindow);
    }

    public void Accept(Guid technicianId)
    {
        ValidateTechnician(technicianId);
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.Accepted);

        Status = WorkOrderStatus.Accepted;
        AcceptedAtUtc = DateTime.UtcNow;
        MarkUpdated();
    }

    public void Reject(Guid technicianId, string reason)
    {
        ValidateTechnician(technicianId);
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.Rejected);

        Status = WorkOrderStatus.Rejected;
        RejectionReason = reason;
        MarkUpdated();
    }

    public void MarkExpired()
    {
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.Expired);
        Status = WorkOrderStatus.Expired;
        MarkUpdated();
    }

    public void StartWork(Guid technicianId)
    {
        ValidateTechnician(technicianId);
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.InProgress);

        Status = WorkOrderStatus.InProgress;
        StartedAtUtc = DateTime.UtcNow;
        MarkUpdated();
    }

    public void CompleteWork(Guid technicianId, string completionNotes)
    {
        ValidateTechnician(technicianId);
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.AwaitingReporter);

        Status = WorkOrderStatus.AwaitingReporter;
        TechnicianCompletedAtUtc = DateTime.UtcNow;
        CompletionNotes = completionNotes;
        MarkUpdated();
    }

    public void ReporterVerify(Guid reporterId)
    {
        ValidateReporter(reporterId);
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.Completed);

        Status = WorkOrderStatus.Completed;
        ClosedAtUtc = DateTime.UtcNow;
        MarkUpdated();
    }

    public void ReporterReopen(Guid reporterId, string reason)
    {
        ValidateReporter(reporterId);
        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.Reopened);

        Status = WorkOrderStatus.Reopened;
        MarkUpdated();
    }

    public void Cancel(Guid requesterId)
    {
        if (requesterId != ReporterId)
        {
            throw new ForbiddenException("Only the issue reporter can cancel this work order.");
        }

        WorkOrderStateMachine.ValidateTransition(Status, WorkOrderStatus.Cancelled);
        Status = WorkOrderStatus.Cancelled;
        MarkUpdated();
    }

    private void ValidateTechnician(Guid technicianId)
    {
        if (AssignedTechnicianId != technicianId)
        {
            throw new ForbiddenException("Only the assigned technician can perform this action.");
        }
    }

    private void ValidateReporter(Guid reporterId)
    {
        if (ReporterId != reporterId)
        {
            throw new ForbiddenException("Only the issue reporter can perform this action.");
        }
    }
}

public static class WorkOrderStateMachine
{
    private static readonly Dictionary<WorkOrderStatus, HashSet<WorkOrderStatus>> AllowedTransitions = new()
    {
        [WorkOrderStatus.AwaitingResponse] = [WorkOrderStatus.Accepted, WorkOrderStatus.Rejected, WorkOrderStatus.Expired, WorkOrderStatus.Cancelled],
        [WorkOrderStatus.Accepted] = [WorkOrderStatus.ReadyToStart, WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled],
        [WorkOrderStatus.ReadyToStart] = [WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled],
        [WorkOrderStatus.InProgress] = [WorkOrderStatus.TechnicianCompleted, WorkOrderStatus.AwaitingReporter],
        [WorkOrderStatus.TechnicianCompleted] = [WorkOrderStatus.AwaitingReporter],
        [WorkOrderStatus.AwaitingReporter] = [WorkOrderStatus.Completed, WorkOrderStatus.Reopened],
        [WorkOrderStatus.Reopened] = [WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled],
        [WorkOrderStatus.Rejected] = [],
        [WorkOrderStatus.Expired] = [],
        [WorkOrderStatus.Cancelled] = [],
        [WorkOrderStatus.Completed] = []
    };

    public static void ValidateTransition(WorkOrderStatus current, WorkOrderStatus next)
    {
        if (!AllowedTransitions.TryGetValue(current, out var validTargets) || !validTargets.Contains(next))
        {
            throw new ConflictException($"Invalid work order state transition from '{current}' to '{next}'.");
        }
    }
}
