using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

/// <summary>A scored risk result produced by the ML pipeline for a student at a point in time.</summary>
public class RiskScore : AuditableEntity
{
    public int StudentProfileId { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public double Probability { get; set; }
    public RiskLevel Level { get; set; }
    public DateTime ScoredAtUtc { get; set; }
    public string? ModelVersion { get; set; }
}
