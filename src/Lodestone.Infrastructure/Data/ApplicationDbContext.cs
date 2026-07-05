using Lodestone.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Data;

/// <summary>EF Core + Identity context. Only Infrastructure and migrations touch this type directly.</summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<CounselorProfile> CounselorProfiles => Set<CounselorProfile>();
    public DbSet<VolunteerProfile> VolunteerProfiles => Set<VolunteerProfile>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<RiskScore> RiskScores => Set<RiskScore>();
    public DbSet<RiskQueueEntry> RiskQueueEntries => Set<RiskQueueEntry>();
    public DbSet<Nudge> Nudges => Set<Nudge>();
    public DbSet<ForumCategory> ForumCategories => Set<ForumCategory>();
    public DbSet<ForumPost> ForumPosts => Set<ForumPost>();
    public DbSet<ForumComment> ForumComments => Set<ForumComment>();
    public DbSet<ForumFlag> ForumFlags => Set<ForumFlag>();
    public DbSet<MoodJournalEntry> MoodJournalEntries => Set<MoodJournalEntry>();
    public DbSet<CounselorAvailabilitySlot> CounselorAvailabilitySlots => Set<CounselorAvailabilitySlot>();
    public DbSet<CounselorBooking> CounselorBookings => Set<CounselorBooking>();
    public DbSet<CrisisResource> CrisisResources => Set<CrisisResource>();
    public DbSet<CounselorSessionReport> CounselorSessionReports => Set<CounselorSessionReport>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Apply all IEntityTypeConfiguration<T> in this assembly.
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
