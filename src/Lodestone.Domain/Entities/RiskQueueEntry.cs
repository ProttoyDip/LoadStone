using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

/// <summary>An at-risk student surfaced to counselors for review/triage.</summary>
public class RiskQueueEntry : AuditableEntity
{
    public int StudentProfileId { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public int RiskScoreId { get; set; }
    public RiskScore? RiskScore { get; set; }

    public RiskLevel Level { get; set; }
    public bool IsResolved { get; set; }
    public string? ResolvedByUserId { get; set; }
    public DateTime? ResolvedAtUtc { get; set; }
}
