using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Technicians.Domain;

public enum VerificationStatus
{
    Unverified = 0,
    PendingReview = 1,
    Probationary = 2,
    Verified = 3,
    Suspended = 4,
    Rejected = 5
}

public enum EvidenceType
{
    FormalQualification = 1,
    LicenseCertificate = 2,
    ExperiencePortfolio = 3,
    ReferenceLetter = 4
}

public class TechnicianEvidence : Entity<Guid>
{
    public Guid TechnicianId { get; private set; }
    public EvidenceType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string DocumentUrl { get; private set; } = string.Empty;
    public bool IsVerifiedByAdmin { get; private set; }
    public DateTime? VerifiedAtUtc { get; private set; }
    public Guid? VerifiedByAdminId { get; private set; }

    private TechnicianEvidence() { }

    public TechnicianEvidence(Guid id, Guid technicianId, EvidenceType type, string title, string documentUrl) : base(id)
    {
        TechnicianId = technicianId;
        Type = type;
        Title = title;
        DocumentUrl = documentUrl;
        IsVerifiedByAdmin = false;
    }

    public void Verify(Guid adminId)
    {
        IsVerifiedByAdmin = true;
        VerifiedByAdminId = adminId;
        VerifiedAtUtc = DateTime.UtcNow;
        MarkUpdated();
    }
}

public class TechnicianProfile : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string Bio { get; private set; } = string.Empty;
    public int YearsOfExperience { get; private set; }
    public List<string> Skills { get; private set; } = [];
    public VerificationStatus Status { get; private set; } = VerificationStatus.Unverified;
    public bool IsAvailable { get; private set; } = true;
    public List<TechnicianEvidence> EvidenceList { get; private set; } = [];

    private TechnicianProfile() { }

    public TechnicianProfile(Guid id, Guid userId, string bio, int yearsOfExperience, IEnumerable<string> skills) : base(id)
    {
        UserId = userId;
        Bio = bio;
        YearsOfExperience = yearsOfExperience;
        Skills = skills.ToList();
        Status = VerificationStatus.PendingReview;
    }

    public void UpdateVerificationStatus(VerificationStatus newStatus)
    {
        Status = newStatus;
        MarkUpdated();
    }

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
        MarkUpdated();
    }
}
