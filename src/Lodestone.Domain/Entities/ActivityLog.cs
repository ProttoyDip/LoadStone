using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

/// <summary>A behavioral signal record used as raw input for risk scoring.</summary>
public class ActivityLog : BaseEntity
{
    public int StudentProfileId { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public DateTime OccurredAtUtc { get; set; }
    public int LoginCount { get; set; }
    public int ForumInteractions { get; set; }
    public int CourseInteractions { get; set; }
    public int DaysSinceLastAccess { get; set; }
    public int AssignmentsLateCount { get; set; }
}
