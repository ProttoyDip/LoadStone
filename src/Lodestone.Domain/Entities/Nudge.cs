using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

/// <summary>A supportive prompt sent to a student when disengagement is detected.</summary>
public class Nudge : AuditableEntity
{
    public int StudentProfileId { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public string Message { get; set; } = string.Empty;
    public NudgeStatus Status { get; set; } = NudgeStatus.Pending;
    public DateTime? SentAtUtc { get; set; }
}
