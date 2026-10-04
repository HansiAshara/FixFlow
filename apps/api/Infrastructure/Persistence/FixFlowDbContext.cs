using FixFlow.Api.Modules.Administration.Domain;
using FixFlow.Api.Modules.Identity.Domain;
using FixFlow.Api.Modules.Issues.Domain;
using FixFlow.Api.Modules.Notifications.Domain;
using FixFlow.Api.Modules.Performance.Domain;
using FixFlow.Api.Modules.Resolutions.Domain;
using FixFlow.Api.Modules.Technicians.Domain;
using FixFlow.Api.Modules.WorkOrders.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FixFlow.Api.Infrastructure.Persistence;

public class FixFlowDbContext(DbContextOptions<FixFlowDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<TechnicianProfile> TechnicianProfiles => Set<TechnicianProfile>();
    public DbSet<TechnicianEvidence> TechnicianEvidence => Set<TechnicianEvidence>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Resolution> Resolutions => Set<Resolution>();
    public DbSet<TechnicianPerformance> TechnicianPerformances => Set<TechnicianPerformance>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AdminAuditLog> AdminAuditLogs => Set<AdminAuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Issues module configuration
        builder.Entity<Issue>(b =>
        {
            b.ToTable("issues");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.RawDescription).HasMaxLength(4000).IsRequired();
            b.Property(x => x.Location).HasMaxLength(200).IsRequired();
            b.Property(x => x.Category).HasMaxLength(100);
            b.Property(x => x.RequiredSkills).HasColumnType("text[]");
            b.Property(x => x.Tags).HasColumnType("text[]");
            b.HasIndex(x => x.ReporterId);
            b.HasIndex(x => x.Status);
        });

        // Technicians module configuration
        builder.Entity<TechnicianProfile>(b =>
        {
            b.ToTable("technician_profiles");
            b.HasKey(x => x.Id);
            b.Property(x => x.Bio).HasMaxLength(1000);
            b.Property(x => x.Skills).HasColumnType("text[]");
            b.HasIndex(x => x.UserId).IsUnique();
            b.HasMany(x => x.EvidenceList).WithOne().HasForeignKey(e => e.TechnicianId);
        });

        builder.Entity<TechnicianEvidence>(b =>
        {
            b.ToTable("technician_evidence");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.DocumentUrl).HasMaxLength(1000).IsRequired();
        });

        // WorkOrders module configuration
        builder.Entity<WorkOrder>(b =>
        {
            b.ToTable("work_orders");
            b.HasKey(x => x.Id);
            b.Property(x => x.RejectionReason).HasMaxLength(1000);
            b.Property(x => x.CompletionNotes).HasMaxLength(2000);
            b.HasIndex(x => x.IssueId);
            b.HasIndex(x => x.AssignedTechnicianId);
            b.HasIndex(x => x.Status);
        });

        // Resolutions module configuration
        builder.Entity<Resolution>(b =>
        {
            b.ToTable("resolutions");
            b.HasKey(x => x.Id);
            b.Property(x => x.Feedback).HasMaxLength(2000);
            b.Property(x => x.ReopenReason).HasMaxLength(1000);
            b.HasIndex(x => x.WorkOrderId);
        });

        // Performance module configuration
        builder.Entity<TechnicianPerformance>(b =>
        {
            b.ToTable("technician_performance");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.TechnicianId).IsUnique();
        });

        // Notifications module configuration
        builder.Entity<Notification>(b =>
        {
            b.ToTable("notifications");
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Message).HasMaxLength(2000).IsRequired();
            b.HasIndex(x => x.RecipientUserId);
            b.HasIndex(x => x.IsRead);
        });

        // Administration module configuration
        builder.Entity<AdminAuditLog>(b =>
        {
            b.ToTable("admin_audit_logs");
            b.HasKey(x => x.Id);
            b.Property(x => x.Action).HasMaxLength(100).IsRequired();
            b.Property(x => x.TargetEntityType).HasMaxLength(100).IsRequired();
            b.HasIndex(x => x.AdminUserId);
        });
    }
}
