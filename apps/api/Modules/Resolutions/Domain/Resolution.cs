using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Resolutions.Domain;

public class Resolution : Entity<Guid>, IAggregateRoot
{
    public Guid WorkOrderId { get; private set; }
    public Guid ReporterId { get; private set; }
    public bool IsVerified { get; private set; }
    public int? Rating { get; private set; }
    public string? Feedback { get; private set; }
    public bool WasReopened { get; private set; }
    public string? ReopenReason { get; private set; }

    private Resolution() { }

    public Resolution(Guid id, Guid workOrderId, Guid reporterId) : base(id)
    {
        WorkOrderId = workOrderId;
        ReporterId = reporterId;
    }

    public void Verify(int rating, string? feedback)
    {
        IsVerified = true;
        Rating = Math.Clamp(rating, 1, 5);
        Feedback = feedback;
        MarkUpdated();
    }

    public void Reopen(string reason)
    {
        WasReopened = true;
        ReopenReason = reason;
        MarkUpdated();
    }
}
