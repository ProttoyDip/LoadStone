using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

public class StudentProfile : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public string? StudentNumber { get; set; }
    public string? Program { get; set; }
    public int EnrollmentYear { get; set; }

    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public ICollection<RiskScore> RiskScores { get; set; } = new List<RiskScore>();
}
