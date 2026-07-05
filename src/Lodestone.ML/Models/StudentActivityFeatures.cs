using Microsoft.ML.Data;

namespace Lodestone.ML.Models;

/// <summary>Feature vector fed to the ML.NET model. Derived from OULAD-style behavioral signals.</summary>
public class StudentActivityFeatures
{
    [LoadColumn(0)] public float LoginFrequency { get; set; }
    [LoadColumn(1)] public float ActivitySpanDays { get; set; }
    [LoadColumn(2)] public float DaysSinceLastAccess { get; set; }
    [LoadColumn(3)] public float ForumParticipation { get; set; }
    [LoadColumn(4)] public float CourseParticipation { get; set; }
    [LoadColumn(5)] public float AssignmentLateness { get; set; }

    // Training label: true = disengaged / at-risk.
    [LoadColumn(6)] public bool IsAtRisk { get; set; }
}
