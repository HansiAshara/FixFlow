using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Administration.Domain;

public class AdminAuditLog : Entity<Guid>, IAggregateRoot
{
    public Guid AdminUserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string TargetEntityType { get; private set; } = string.Empty;
    public Guid TargetEntityId { get; private set; }
    public string DetailsJson { get; private set; } = string.Empty;

    private AdminAuditLog() { }

    public AdminAuditLog(Guid id, Guid adminUserId, string action, string targetEntityType, Guid targetEntityId, string detailsJson)
        : base(id)
    {
        AdminUserId = adminUserId;
        Action = action;
        TargetEntityType = targetEntityType;
        TargetEntityId = targetEntityId;
        DetailsJson = detailsJson;
    }
}
