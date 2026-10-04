using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Modules.Issues.Domain;
using FixFlow.Api.Modules.Matching.Domain;
using FixFlow.Api.Modules.Performance.Domain;
using FixFlow.Api.Modules.WorkOrders.Domain;
using FluentAssertions;
using Xunit;

namespace FixFlow.UnitTests;

public class DomainRuleTests
{
    [Theory]
    [InlineData(IssueSeverity.Low, IssueRiskLevel.Low, 2)]
    [InlineData(IssueSeverity.Medium, IssueRiskLevel.Medium, 5)]
    [InlineData(IssueSeverity.High, IssueRiskLevel.High, 8)]
    [InlineData(IssueSeverity.Critical, IssueRiskLevel.Hazardous, 10)]
    public void IssuePriorityCalculator_ShouldCalculateDeterministicPriority(
        IssueSeverity severity,
        IssueRiskLevel riskLevel,
        int expectedPriority)
    {
        var priority = IssuePriorityCalculator.Calculate(severity, riskLevel);
        priority.Should().Be(expectedPriority);
    }

    [Fact]
    public void WorkOrderStateMachine_ShouldAllowValidTransitions()
    {
        // AwaitingResponse -> Accepted -> ReadyToStart -> InProgress -> AwaitingReporter -> Completed
        var act1 = () => WorkOrderStateMachine.ValidateTransition(WorkOrderStatus.AwaitingResponse, WorkOrderStatus.Accepted);
        var act2 = () => WorkOrderStateMachine.ValidateTransition(WorkOrderStatus.Accepted, WorkOrderStatus.InProgress);
        var act3 = () => WorkOrderStateMachine.ValidateTransition(WorkOrderStatus.InProgress, WorkOrderStatus.AwaitingReporter);
        var act4 = () => WorkOrderStateMachine.ValidateTransition(WorkOrderStatus.AwaitingReporter, WorkOrderStatus.Completed);

        act1.Should().NotThrow();
        act2.Should().NotThrow();
        act3.Should().NotThrow();
        act4.Should().NotThrow();
    }

    [Fact]
    public void WorkOrderStateMachine_ShouldRejectInvalidTransitions()
    {
        // Completed -> InProgress (Invalid)
        var act1 = () => WorkOrderStateMachine.ValidateTransition(WorkOrderStatus.Completed, WorkOrderStatus.InProgress);
        // AwaitingResponse -> Completed (Invalid)
        var act2 = () => WorkOrderStateMachine.ValidateTransition(WorkOrderStatus.AwaitingResponse, WorkOrderStatus.Completed);

        act1.Should().Throw<ConflictException>();
        act2.Should().Throw<ConflictException>();
    }

    [Fact]
    public void MatchingAlgorithm_ShouldCalculateWeightedScoreAccurately()
    {
        // Skill: 1.0 (40%), Experience: 0.8 (25%), History: 0.9 (20%), Availability: 1.0 (15%)
        // Score = 0.40 + 0.20 + 0.18 + 0.15 = 0.93
        var score = MatchingAlgorithm.CalculateScore(1.0, 0.8, 0.9, 1.0);
        score.Should().Be(0.93);
    }

    [Fact]
    public void PerformanceCalculator_ShouldCalculateExpectedScoresWithPenalties()
    {
        var score = PerformanceCalculator.CalculateOverallScore(
            completionRate: 1.0,
            responseRate: 1.0,
            avgRating: 5.0,
            reopenedCount: 0,
            totalAssigned: 10
        );

        score.Should().Be(1.0);
    }
}
