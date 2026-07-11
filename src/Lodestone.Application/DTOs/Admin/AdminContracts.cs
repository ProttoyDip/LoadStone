namespace Lodestone.Application.DTOs.Admin;

public enum AdminSectionType
{
    Dashboard,
    Students,
    Counselors,
    Volunteers,
    Users,
    RiskMonitoring,
    ForumModeration,
    CounselorBookings,
    MoodJournals,
    Reports,
    Analytics,
    MachineLearning,
    BackgroundJobs,
    Notifications,
    AuditLogs,
    Settings,
    Profile
}

public record AdminShellDto(
    string AdminName,
    int UnreadNotifications,
    int UnreadMessages,
    string ModelHealth,
    string CurrentDateLabel,
    string ProfileImageUrl);

public record AdminStatCardDto(
    string Title,
    string Value,
    string Delta,
    string DeltaClass,
    string Icon,
    string GradientClass,
    string SparklineJson);

public record AdminMiniMetricDto(string Label, string Value, string? Tone = null);

public record AdminActivityItemDto(
    string Title,
    string Subtitle,
    string TimeLabel,
    string Icon,
    string ToneClass);

public record AdminChartDto(
    string Id,
    string Title,
    string Subtitle,
    string Type,
    string LabelsJson,
    string DatasetsJson);

public record AdminMlPanelDto(
    string CurrentModel,
    string Accuracy,
    string Precision,
    string Recall,
    string F1Score,
    string RocAuc,
    string LastTrainingDate,
    int TotalPredictionsToday,
    int PredictionQueue,
    string ModelHealth,
    bool RetrainEnabled);

public record AdminHangfirePanelDto(
    int RunningJobs,
    int CompletedJobs,
    int FailedJobs,
    int UpcomingJobs,
    IReadOnlyList<AdminActivityItemDto> RecentLogs);

public record AdminSectionColumnDto(string Key, string Label, bool IsNumeric = false, string? CssClass = null);

public record AdminSectionActionDto(string Label, string Url, string Icon, string CssClass, string? Method = null);

public record AdminSectionRowDto(
    string Id,
    string PrimaryLabel,
    string SecondaryLabel,
    string? BadgeText,
    string? BadgeClass,
    IReadOnlyDictionary<string, string> Cells,
    IReadOnlyList<AdminSectionActionDto> Actions);

public record AdminSectionPageDto(
    AdminSectionType Section,
    string Title,
    string Subtitle,
    string SearchPlaceholder,
    bool ShowExportButtons,
    bool ShowFilters,
    IReadOnlyList<AdminMiniMetricDto> Metrics,
    IReadOnlyList<AdminSectionColumnDto> Columns,
    IReadOnlyList<AdminSectionRowDto> Rows,
    IReadOnlyList<AdminActivityItemDto> Activity,
    string? EmptyStateMessage = null);

public record AdminDashboardDto(
    AdminShellDto Shell,
    IReadOnlyList<AdminStatCardDto> Stats,
    IReadOnlyList<AdminChartDto> Charts,
    IReadOnlyList<AdminActivityItemDto> RecentActivity,
    AdminMlPanelDto MlPanel,
    AdminHangfirePanelDto HangfirePanel,
    IReadOnlyList<AdminSectionRowDto> RiskRows,
    IReadOnlyList<AdminSectionRowDto> BookingRows,
    IReadOnlyList<AdminSectionRowDto> NotificationRows);
