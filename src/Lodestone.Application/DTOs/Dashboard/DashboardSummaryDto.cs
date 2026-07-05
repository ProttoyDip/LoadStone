namespace Lodestone.Application.DTOs.Dashboard;

/// <summary>Aggregated figures rendered on the dashboard and fed to Chart.js.</summary>
public record DashboardSummaryDto(
    int TotalStudents,
    int AtRiskStudents,
    int OpenQueueItems,
    int UpcomingBookings,
    IReadOnlyList<RiskTrendPointDto> RiskTrend);

public record RiskTrendPointDto(DateTime DateUtc, int HighRiskCount);
