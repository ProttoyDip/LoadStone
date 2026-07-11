using System.Globalization;
using Lodestone.Application.DTOs.Admin;
using Lodestone.Application.Interfaces;
using Lodestone.Domain.Constants;
using Lodestone.Domain.Entities;
using Lodestone.Domain.Enums;
using Lodestone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AdminDashboardService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<AdminShellDto> GetShellAsync(CancellationToken cancellationToken = default)
    {
        var displayName = _currentUserService.UserName ?? "Admin";
        if (_currentUserService.IsAuthenticated && !string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            displayName = await _context.Users
                .AsNoTracking()
                .Where(user => user.Id == _currentUserService.UserId)
                .Select(user => string.IsNullOrWhiteSpace(user.FullName) ? user.UserName ?? string.Empty : user.FullName)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = _currentUserService.UserName ?? "Admin";
            }
        }

        var unreadNotifications = await _context.Notifications
            .Where(notification => !_currentUserService.IsAuthenticated || notification.RecipientUserId == _currentUserService.UserId)
            .CountAsync(notification => !notification.IsRead, cancellationToken);

        return new AdminShellDto(
            AdminName: displayName,
            UnreadNotifications: unreadNotifications,
            UnreadMessages: 4,
            ModelHealth: "Healthy",
            CurrentDateLabel: DateTime.Now.ToString("dddd, dd MMM yyyy", CultureInfo.InvariantCulture),
            ProfileImageUrl: "/images/admin-avatar.svg");
    }

    public async Task<AdminDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var shell = await GetShellAsync(cancellationToken);
        var riskRows = await BuildRiskRowsAsync(cancellationToken);
        var bookingRows = await BuildBookingRowsAsync(cancellationToken);
        var notificationRows = await BuildNotificationRowsAsync(cancellationToken);

        return new AdminDashboardDto(
            Shell: shell,
            Stats: BuildStats(riskRows, bookingRows, notificationRows),
            Charts: BuildCharts(),
            RecentActivity: BuildRecentActivity(),
            MlPanel: BuildMlPanel(),
            HangfirePanel: BuildHangfirePanel(),
            RiskRows: riskRows,
            BookingRows: bookingRows,
            NotificationRows: notificationRows);
    }

    public async Task<AdminSectionPageDto> GetSectionAsync(AdminSectionType section, CancellationToken cancellationToken = default)
    {
        var shell = await GetShellAsync(cancellationToken);
        return section switch
        {
            AdminSectionType.Students => BuildStudentsPage(shell),
            AdminSectionType.Counselors => BuildCounselorsPage(shell),
            AdminSectionType.Volunteers => BuildVolunteersPage(shell),
            AdminSectionType.Users => BuildUsersPage(shell),
            AdminSectionType.RiskMonitoring => await BuildRiskMonitoringPageAsync(shell, cancellationToken),
            AdminSectionType.ForumModeration => BuildForumModerationPage(shell),
            AdminSectionType.CounselorBookings => BuildBookingPage(shell),
            AdminSectionType.MoodJournals => BuildMoodJournalsPage(shell),
            AdminSectionType.Reports => BuildReportsPage(shell),
            AdminSectionType.Analytics => BuildAnalyticsPage(shell),
            AdminSectionType.MachineLearning => BuildMachineLearningPage(shell),
            AdminSectionType.BackgroundJobs => BuildBackgroundJobsPage(shell),
            AdminSectionType.Notifications => BuildNotificationsPage(shell),
            AdminSectionType.AuditLogs => await BuildAuditLogsPageAsync(shell, cancellationToken),
            AdminSectionType.Settings => BuildSettingsPage(shell),
            AdminSectionType.Profile => BuildProfilePage(shell),
            _ => BuildStudentsPage(shell)
        };
    }

    private static IReadOnlyList<AdminStatCardDto> BuildStats(
        IReadOnlyList<AdminSectionRowDto> riskRows,
        IReadOnlyList<AdminSectionRowDto> bookingRows,
        IReadOnlyList<AdminSectionRowDto> notificationRows)
    {
        return new[]
        {
            new AdminStatCardDto("Total Students", "1,248", "+8.4%", "text-success", "bi-people", "gradient-teal", "[6,7,8,10,11,12,13]"),
            new AdminStatCardDto("Active Students Today", "972", "+4.2%", "text-success", "bi-activity", "gradient-gold", "[8,8,9,10,10,11,12]"),
            new AdminStatCardDto("High Risk Students", "38", "-2.8%", "text-danger", "bi-exclamation-triangle", "gradient-rose", "[15,14,14,13,12,10,9]"),
            new AdminStatCardDto("Medium Risk Students", "114", "+1.3%", "text-warning", "bi-graph-up", "gradient-sand", "[11,10,11,12,12,13,13]"),
            new AdminStatCardDto("Low Risk Students", "1,096", "+2.9%", "text-success", "bi-shield-check", "gradient-forest", "[9,10,10,11,12,12,13]"),
            new AdminStatCardDto("Counselors", "18", "+0.0%", "text-muted", "bi-person-badge", "gradient-ink", "[6,6,6,7,7,7,8]"),
            new AdminStatCardDto("Volunteers", "43", "+5.1%", "text-success", "bi-hand-thumbs-up", "gradient-clay", "[5,5,6,6,7,7,8]"),
            new AdminStatCardDto("Forum Posts", "1,542", "+12.7%", "text-success", "bi-chat-square-text", "gradient-ocean", "[12,12,13,13,14,15,16]"),

            new AdminStatCardDto("Pending Reviews", "26", "-6.0%", "text-danger", "bi-inbox", "gradient-amber", "[12,11,10,10,9,8,7]"),
            new AdminStatCardDto("Unread Notifications", notificationRows.Count.ToString(CultureInfo.InvariantCulture), "+3.4%", "text-success", "bi-bell", "gradient-indigo", "[2,3,4,5,5,6,7]")
        };
    }

    private static IReadOnlyList<AdminChartDto> BuildCharts()
    {
        string labels = """["Mon","Tue","Wed","Thu","Fri","Sat","Sun"]""";
        return new[]
        {
            new AdminChartDto("riskDistributionChart", "Risk Distribution", "Students by current risk level", "doughnut", labels, """{"labels":["Low","Medium","High"],"datasets":[{"data":[74,18,8],"backgroundColor":["#3C5647","#E0A85A","#BC5138"],"borderWidth":0}]}"""),
            new AdminChartDto("forumActivityChart", "Forum Activity", "Posts, comments, and flags", "bar", labels, """{"labels":["Mon","Tue","Wed","Thu","Fri","Sat","Sun"],"datasets":[{"label":"Posts","data":[32,28,41,36,39,35,44],"backgroundColor":"#BC5138"},{"label":"Flags","data":[4,6,3,5,7,4,6],"backgroundColor":"#E0A85A"}]}"""),
        };
    }

    private static IReadOnlyList<AdminActivityItemDto> BuildRecentActivity()
        => new[]
        {
            new AdminActivityItemDto("Student completed assignment", "ENG 201 submission was marked complete", "5 min ago", "bi-journal-check", "tone-success"),
            new AdminActivityItemDto("Risk score updated", "Student risk moved from medium to high", "12 min ago", "bi-arrow-repeat", "tone-warning"),
            new AdminActivityItemDto("Counselor accepted booking", "Counselor Rahman confirmed a session", "18 min ago", "bi-calendar-check", "tone-forest"),
            new AdminActivityItemDto("Volunteer replied in forum", "Helpful reply added to academic stress thread", "29 min ago", "bi-chat-dots", "tone-clay"),
            new AdminActivityItemDto("Journal created", "Private mood journal entry saved", "41 min ago", "bi-pencil-square", "tone-indigo"),
            new AdminActivityItemDto("Forum reported", "Moderation queue received a new report", "1 hr ago", "bi-flag", "tone-rose"),
            new AdminActivityItemDto("Admin changed settings", "Notification routing updated", "2 hr ago", "bi-gear", "tone-sand")
        };

    private static AdminMlPanelDto BuildMlPanel()
        => new(
            CurrentModel: "risk-model.zip",
            Accuracy: "93.4%",
            Precision: "91.7%",
            Recall: "89.8%",
            F1Score: "90.7%",
            RocAuc: "0.94",
            LastTrainingDate: "2026-07-10",
            TotalPredictionsToday: 428,
            PredictionQueue: 12,
            ModelHealth: "Healthy",
            RetrainEnabled: true);

    private static AdminHangfirePanelDto BuildHangfirePanel()
        => new(
            RunningJobs: 3,
            CompletedJobs: 42,
            FailedJobs: 1,
            UpcomingJobs: 5,
            RecentLogs: new[]
            {

                new AdminActivityItemDto("Notification fan-out", "Processed 84 recipients", "14 min ago", "bi-bell", "tone-indigo"),
                new AdminActivityItemDto("Forum moderation sweep", "Two posts flagged for review", "27 min ago", "bi-shield-exclamation", "tone-warning")
            });

    private async Task<IReadOnlyList<AdminSectionRowDto>> BuildRiskRowsAsync(CancellationToken cancellationToken)
    {
        var records = await _context.RiskScores
            .AsNoTracking()
            .Include(score => score.StudentProfile)
                .ThenInclude(student => student!.User)
            .OrderByDescending(score => score.ScoredAtUtc)
            .Take(8)
            .ToListAsync(cancellationToken);

        var rows = records.Select(score => new AdminSectionRowDto(
            Id: score.Id.ToString(CultureInfo.InvariantCulture),
            PrimaryLabel: score.StudentProfile!.User!.FullName,
            SecondaryLabel: score.StudentProfile.StudentNumber ?? "No student number",
            BadgeText: score.Level.ToString(),
            BadgeClass: RiskBadge(score.Level),
            Cells: new Dictionary<string, string>
            {
                ["studentId"] = score.StudentProfile.StudentNumber ?? "N/A",
                ["risk"] = $"{score.Probability:P1}",
                ["level"] = score.Level.ToString(),
                ["lastActive"] = score.StudentProfile!.CreatedAtUtc.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
                ["predictionTime"] = score.ScoredAtUtc.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture),
                ["counselor"] = score.StudentProfile.User!.FullName,
                ["status"] = score.Level == RiskLevel.High ? "Escalated" : "Monitoring"
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Assign", "#", "bi-person-plus", "btn btn-sm btn-outline-success")
            })).ToList();

        return rows.Count > 0 ? rows : BuildRiskSampleRows();
    }

    private async Task<IReadOnlyList<AdminSectionRowDto>> BuildBookingRowsAsync(CancellationToken cancellationToken)
    {
        var records = await _context.CounselorBookings
            .AsNoTracking()
            .Include(booking => booking.StudentProfile)
                .ThenInclude(student => student!.User)
            .Include(booking => booking.CounselorProfile)
                .ThenInclude(counselor => counselor!.User)
            .OrderByDescending(booking => booking.ScheduledForUtc)
            .Take(6)
            .ToListAsync(cancellationToken);

        var rows = records.Select(booking => new AdminSectionRowDto(
            Id: booking.Id.ToString(CultureInfo.InvariantCulture),
            PrimaryLabel: booking.StudentProfile!.User!.FullName,
            SecondaryLabel: booking.CounselorProfile!.User!.FullName,
            BadgeText: booking.Status.ToString(),
            BadgeClass: BookingBadge(booking.Status),
            Cells: new Dictionary<string, string>
            {
                ["pending"] = booking.Status == BookingStatus.Requested ? "Yes" : "No",
                ["approved"] = booking.Status == BookingStatus.Confirmed ? "Yes" : "No",
                ["rejected"] = booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.NoShow ? "Yes" : "No",
                ["completed"] = booking.Status == BookingStatus.Completed ? "Yes" : "No",
                ["scheduled"] = booking.ScheduledForUtc.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture)
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Approve", "#", "bi-check-lg", "btn btn-sm btn-outline-success"),
                new AdminSectionActionDto("Reject", "#", "bi-x-lg", "btn btn-sm btn-outline-danger")
            })).ToList();

        return rows.Count > 0 ? rows : BuildBookingSampleRows();
    }

    private async Task<IReadOnlyList<AdminSectionRowDto>> BuildNotificationRowsAsync(CancellationToken cancellationToken)
    {
        var records = await _context.Notifications
            .AsNoTracking()
            .OrderByDescending(notification => notification.CreatedAtUtc)
            .Take(6)
            .ToListAsync(cancellationToken);

        var rows = records.Select(notification => new AdminSectionRowDto(
            Id: notification.Id.ToString(CultureInfo.InvariantCulture),
            PrimaryLabel: notification.Title,
            SecondaryLabel: notification.Message,
            BadgeText: notification.IsRead ? "Read" : "Unread",
            BadgeClass: notification.IsRead ? "badge text-bg-secondary" : "badge text-bg-primary",
            Cells: new Dictionary<string, string>
            {
                ["type"] = notification.Type.ToString(),
                ["created"] = notification.CreatedAtUtc.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture),
                ["recipient"] = notification.RecipientUserId,
                ["status"] = notification.IsRead ? "Read" : "Unread"
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Delete", "#", "bi-trash", "btn btn-sm btn-outline-danger")
            })).ToList();

        return rows.Count > 0 ? rows : BuildNotificationSampleRows();
    }

    private async Task<AdminSectionPageDto> BuildRiskMonitoringPageAsync(AdminShellDto shell, CancellationToken cancellationToken)
    {
        var rows = await BuildRiskRowsAsync(cancellationToken);
        return new AdminSectionPageDto(
            AdminSectionType.RiskMonitoring,
            "Risk Monitoring",
            "Track disengagement signals, review assigned counselors, and export triage queues.",
            "Search risk queue",
            ShowExportButtons: true,
            ShowFilters: true,
            Metrics: new[]
            {
                new AdminMiniMetricDto("High risk", "38", "text-danger"),
                new AdminMiniMetricDto("Medium risk", "114", "text-warning"),
                new AdminMiniMetricDto("Resolved today", "14", "text-success")
            },
            Columns: RiskColumns(),
            Rows: rows,
            Activity: BuildRecentActivity(),
            EmptyStateMessage: "No risk items were found. Sample rows are shown when the live database is empty.");
    }

    private AdminSectionPageDto BuildStudentsPage(AdminShellDto shell)
        => new(
            AdminSectionType.Students,
            "Student Management",
            "Review student records, risk levels, and support status.",
            "Search students",
            ShowExportButtons: false,
            ShowFilters: true,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Total students", "1,248"),
                new AdminMiniMetricDto("Active today", "972", "text-success"),
                new AdminMiniMetricDto("Disabled", "21", "text-danger")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("studentId", "Student ID"),
                new AdminSectionColumnDto("photo", "Photo"),
                new AdminSectionColumnDto("name", "Name"),
                new AdminSectionColumnDto("department", "Department"),
                new AdminSectionColumnDto("email", "Email"),
                new AdminSectionColumnDto("risk", "Risk Level"),
                new AdminSectionColumnDto("lastLogin", "Last Login"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildStudentSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildCounselorsPage(AdminShellDto shell)
        => new(
            AdminSectionType.Counselors,
            "Counselor Management",
            "Monitor availability, case load, and response times.",
            "Search counselors",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Available", "12", "text-success"),
                new AdminMiniMetricDto("On leave", "2", "text-warning"),
                new AdminMiniMetricDto("Avg response", "18m", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("name", "Name"),
                new AdminSectionColumnDto("availability", "Availability"),
                new AdminSectionColumnDto("cases", "Current Cases", true),
                new AdminSectionColumnDto("sessions", "Completed Sessions", true),
                new AdminSectionColumnDto("bookings", "Pending Bookings", true),
                new AdminSectionColumnDto("response", "Average Response Time"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildCounselorSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildVolunteersPage(AdminShellDto shell)
        => new(
            AdminSectionType.Volunteers,
            "Volunteer Management",
            "Track assignments, forum activity, and escalation support.",
            "Search volunteers",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Approved", "31", "text-success"),
                new AdminMiniMetricDto("Pending", "9", "text-warning"),
                new AdminMiniMetricDto("Active today", "19", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("name", "Name"),
                new AdminSectionColumnDto("assigned", "Assigned Students", true),
                new AdminSectionColumnDto("forum", "Forum Activity", true),
                new AdminSectionColumnDto("resolved", "Reports Resolved", true),
                new AdminSectionColumnDto("status", "Active Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildVolunteerSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildUsersPage(AdminShellDto shell)
        => new(
            AdminSectionType.Users,
            "User Administration",
            "Manage logins, roles, and account status across the university platform.",
            "Search users",
            ShowExportButtons: false,
            ShowFilters: true,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Admins", "1", "text-success"),
                new AdminMiniMetricDto("Counselors", "18", "text-muted"),
                new AdminMiniMetricDto("Students", "1,248", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("name", "Name"),
                new AdminSectionColumnDto("role", "Role"),
                new AdminSectionColumnDto("email", "Email"),
                new AdminSectionColumnDto("lastLogin", "Last Login"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildUserSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildForumModerationPage(AdminShellDto shell)
        => new(
            AdminSectionType.ForumModeration,
            "Forum Moderation",
            "Review reported posts, apply moderation actions, and preserve the flag history.",
            "Search reported posts",
            ShowExportButtons: false,
            ShowFilters: true,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Reported", "14", "text-warning"),
                new AdminMiniMetricDto("Approved", "6", "text-success"),
                new AdminMiniMetricDto("Deleted", "3", "text-danger")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("post", "Reported Post"),
                new AdminSectionColumnDto("reportedBy", "Reported By"),
                new AdminSectionColumnDto("flags", "Flag History", true),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildForumSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildBookingPage(AdminShellDto shell)
        => new(
            AdminSectionType.CounselorBookings,
            "Booking Management",
            "Coordinate counseling appointments and calendar load.",
            "Search bookings",
            ShowExportButtons: true,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Pending", "17", "text-warning"),
                new AdminMiniMetricDto("Approved", "29", "text-success"),
                new AdminMiniMetricDto("Completed", "84", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("student", "Student"),
                new AdminSectionColumnDto("counselor", "Counselor"),
                new AdminSectionColumnDto("pending", "Pending"),
                new AdminSectionColumnDto("approved", "Approved"),
                new AdminSectionColumnDto("rejected", "Rejected"),
                new AdminSectionColumnDto("completed", "Completed"),
                new AdminSectionColumnDto("scheduled", "Scheduled For"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildBookingSampleRows(),
            Activity: BuildRecentActivity(),
            EmptyStateMessage: "Calendar and list view use the same sample booking data on first run.");

    private AdminSectionPageDto BuildMoodJournalsPage(AdminShellDto shell)
        => new(
            AdminSectionType.MoodJournals,
            "Mood Journals",
            "Review private journal cadence and identify engagement dips.",
            "Search journal entries",
            ShowExportButtons: false,
            ShowFilters: true,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Entries today", "78", "text-success"),
                new AdminMiniMetricDto("Average mood", "3.8/5", "text-muted"),
                new AdminMiniMetricDto("Flagged", "5", "text-warning")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("student", "Student"),
                new AdminSectionColumnDto("mood", "Mood"),
                new AdminSectionColumnDto("entry", "Last Entry"),
                new AdminSectionColumnDto("flags", "Flags"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildJournalSampleRows(),
            Activity: BuildRecentActivity());

    private static IReadOnlyList<AdminSectionRowDto> BuildJournalSampleRows()
        => new[]
        {
            CreateJournalRow("Ayesha Rahman", "Calm", "11 Jul 2026", "0"),
            CreateJournalRow("Tanvir Ahmed", "Stressed", "11 Jul 2026", "1"),
            CreateJournalRow("Nusrat Jahan", "Focused", "10 Jul 2026", "0")
        };

    private static AdminSectionRowDto CreateJournalRow(string student, string mood, string entry, string flags)
        => new(
            Id: student,
            PrimaryLabel: student,
            SecondaryLabel: mood,
            BadgeText: mood,
            BadgeClass: mood == "Stressed" ? "badge text-bg-warning" : "badge text-bg-success",
            Cells: new Dictionary<string, string>
            {
                ["student"] = student,
                ["mood"] = mood,
                ["entry"] = entry,
                ["flags"] = flags,
                ["status"] = mood == "Stressed" ? "Needs Review" : "Normal"
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Flag", "#", "bi-flag", "btn btn-sm btn-outline-warning")
            });

    private AdminSectionPageDto BuildReportsPage(AdminShellDto shell)
        => new(
            AdminSectionType.Reports,
            "Reports",
            "Generate QuestPDF exports for risk, engagement, bookings, and forum activity.",
            "Search reports",
            ShowExportButtons: true,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Ready", "5", "text-success"),
                new AdminMiniMetricDto("Queued", "2", "text-warning"),
                new AdminMiniMetricDto("Exports today", "12", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("report", "Report"),
                new AdminSectionColumnDto("scope", "Scope"),
                new AdminSectionColumnDto("format", "Format"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildReportSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildAnalyticsPage(AdminShellDto shell)
        => new(
            AdminSectionType.Analytics,
            "Analytics",
            "Compare departments, view risk heat, and inspect cohort trends.",
            "Search analytics views",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Heatmaps", "4", "text-muted"),
                new AdminMiniMetricDto("Departments tracked", "8", "text-muted"),
                new AdminMiniMetricDto("Faculty comparisons", "12", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("metric", "Metric"),
                new AdminSectionColumnDto("current", "Current"),
                new AdminSectionColumnDto("previous", "Previous"),
                new AdminSectionColumnDto("trend", "Trend"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildAnalyticsSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildMachineLearningPage(AdminShellDto shell)
        => new(
            AdminSectionType.MachineLearning,
            "Machine Learning",
            "Track model health, prediction throughput, and retraining readiness.",
            "Search model outputs",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Accuracy", "93.4%", "text-success"),
                new AdminMiniMetricDto("ROC AUC", "0.94", "text-success"),
                new AdminMiniMetricDto("Queue", "12", "text-warning")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("model", "Model"),
                new AdminSectionColumnDto("accuracy", "Accuracy"),
                new AdminSectionColumnDto("predictions", "Today", true),
                new AdminSectionColumnDto("health", "Health"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildMlSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildBackgroundJobsPage(AdminShellDto shell)
        => new(
            AdminSectionType.BackgroundJobs,
            "Background Jobs",
            "Monitor scheduled jobs, execution logs, and retry status.",
            "Search background jobs",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Running", "3", "text-muted"),
                new AdminMiniMetricDto("Completed", "42", "text-success"),
                new AdminMiniMetricDto("Failed", "1", "text-danger")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("job", "Job"),
                new AdminSectionColumnDto("schedule", "Schedule"),
                new AdminSectionColumnDto("lastRun", "Last Run"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildJobSampleRows(),
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildNotificationsPage(AdminShellDto shell)
        => new(
            AdminSectionType.Notifications,
            "Notification Center",
            "Review unread items, mark them read, and inspect delivery details.",
            "Search notifications",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Unread", shell.UnreadNotifications.ToString(CultureInfo.InvariantCulture), "text-primary"),
                new AdminMiniMetricDto("Delivered today", "96", "text-muted"),
                new AdminMiniMetricDto("Muted", "3", "text-warning")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("title", "Title"),
                new AdminSectionColumnDto("message", "Message"),
                new AdminSectionColumnDto("type", "Type"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: BuildNotificationSampleRows(),
            Activity: BuildRecentActivity());

    private async Task<AdminSectionPageDto> BuildAuditLogsPageAsync(AdminShellDto shell, CancellationToken cancellationToken)
    {
        var records = await _context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(log => log.TimestampUtc)
            .Take(10)
            .ToListAsync(cancellationToken);

        var rows = records.Select(log => new AdminSectionRowDto(
            Id: log.Id.ToString(CultureInfo.InvariantCulture),
            PrimaryLabel: log.Action,
            SecondaryLabel: log.EntityName ?? "System",
            BadgeText: "Audit",
            BadgeClass: "badge text-bg-dark",
            Cells: new Dictionary<string, string>
            {
                ["user"] = log.UserId ?? "System",
                ["entity"] = log.EntityName ?? "N/A",
                ["entityId"] = log.EntityId ?? "N/A",
                ["timestamp"] = log.TimestampUtc.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture)
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Inspect", "#", "bi-search", "btn btn-sm btn-outline-primary")
            })).ToList();

        return new AdminSectionPageDto(
            AdminSectionType.AuditLogs,
            "Audit Logs",
            "Search security and privacy events with date filters.",
            "Search audit entries",
            ShowExportButtons: false,
            ShowFilters: true,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Login events", "142", "text-muted"),
                new AdminMiniMetricDto("Role changes", "6", "text-muted"),
                new AdminMiniMetricDto("Settings changes", "11", "text-muted")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("user", "User"),
                new AdminSectionColumnDto("entity", "Entity"),
                new AdminSectionColumnDto("entityId", "Entity ID"),
                new AdminSectionColumnDto("timestamp", "Timestamp"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: rows.Count > 0 ? rows : BuildAuditSampleRows(),
            Activity: BuildRecentActivity());
    }

    private AdminSectionPageDto BuildSettingsPage(AdminShellDto shell)
        => new(
            AdminSectionType.Settings,
            "Settings",
            "Review notification, privacy, email, and security settings.",
            "Search settings",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("General", "Ready", "text-success"),
                new AdminMiniMetricDto("Email", "Configured", "text-success"),
                new AdminMiniMetricDto("Security", "Hardened", "text-success")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("section", "Section"),
                new AdminSectionColumnDto("value", "Value"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: new[]
            {
                CreateSingleValueRow("general", "General", "University branding, locale, and time zone", "Enabled"),
                CreateSingleValueRow("email", "Email", "SMTP and template settings", "Configured"),
                CreateSingleValueRow("ml", "ML Settings", "Retrain cadence and thresholds", "Healthy"),
                CreateSingleValueRow("privacy", "Privacy", "Audit logging and retention", "Enabled"),
                CreateSingleValueRow("security", "Security", "Session timeout and role policies", "Enabled"),
                CreateSingleValueRow("system", "System Information", "net8.0 / Bootstrap / SignalR", "Online")
            },
            Activity: BuildRecentActivity());

    private AdminSectionPageDto BuildProfilePage(AdminShellDto shell)
        => new(
            AdminSectionType.Profile,
            "Profile",
            "Update the admin profile and password.",
            "Search profile settings",
            ShowExportButtons: false,
            ShowFilters: false,
            Metrics: new[]
            {
                new AdminMiniMetricDto("Name", shell.AdminName),
                new AdminMiniMetricDto("Role", RoleConstants.Admin),
                new AdminMiniMetricDto("Status", "Active")
            },
            Columns: new[]
            {
                new AdminSectionColumnDto("field", "Field"),
                new AdminSectionColumnDto("value", "Value"),
                new AdminSectionColumnDto("status", "Status"),
                new AdminSectionColumnDto("actions", "Actions")
            },
            Rows: new[]
            {
                CreateSingleValueRow("profile-picture", "Profile Picture", "Default avatar in use", "Upload"),
                CreateSingleValueRow("name", "Name", shell.AdminName, "Saved"),
                CreateSingleValueRow("email", "Email", "rashid.cse.20230104102@aust.edu", "Verified"),
                CreateSingleValueRow("role", "Role", RoleConstants.Admin, "Locked"),
                CreateSingleValueRow("password", "Change Password", "Managed through Identity", "Available")
            },
            Activity: BuildRecentActivity());

    private static AdminSectionRowDto CreateSingleValueRow(string id, string label, string value, string status)
        => new(
            Id: id,
            PrimaryLabel: label,
            SecondaryLabel: value,
            BadgeText: status,
            BadgeClass: "badge text-bg-secondary",
            Cells: new Dictionary<string, string>
            {
                ["section"] = label,
                ["value"] = value,
                ["status"] = status
            },
            Actions: Array.Empty<AdminSectionActionDto>());

    private static IReadOnlyList<AdminSectionColumnDto> RiskColumns()
        => new[]
        {
            new AdminSectionColumnDto("studentName", "Student Name"),
            new AdminSectionColumnDto("studentId", "Student ID"),
            new AdminSectionColumnDto("risk", "Current Risk Score", true),
            new AdminSectionColumnDto("level", "Risk Level"),
            new AdminSectionColumnDto("lastActive", "Last Active"),
            new AdminSectionColumnDto("predictionTime", "Prediction Time"),
            new AdminSectionColumnDto("counselor", "Assigned Counselor"),
            new AdminSectionColumnDto("status", "Status"),
            new AdminSectionColumnDto("actions", "Actions")
        };

    private static IReadOnlyList<AdminSectionRowDto> BuildRiskSampleRows()
        => new[]
        {
            Row("Ayesha Rahman", "2023-04102", "0.89", RiskLevel.High, "2026-07-11 08:20", "2026-07-11 08:34", "Dr. M. Hasan", "Escalated"),
            Row("Tanvir Ahmed", "2023-03814", "0.71", RiskLevel.Moderate, "2026-07-11 09:02", "2026-07-11 09:12", "Dr. N. Akter", "Monitoring"),
            Row("Nusrat Jahan", "2022-02751", "0.33", RiskLevel.Low, "2026-07-11 07:54", "2026-07-11 08:11", "Dr. S. Rahman", "Stable")
        };

    private static AdminSectionRowDto Row(string name, string studentId, string risk, RiskLevel level, string lastActive, string predictionTime, string counselor, string status)
        => new(
            Id: studentId,
            PrimaryLabel: name,
            SecondaryLabel: studentId,
            BadgeText: level.ToString(),
            BadgeClass: RiskBadge(level),
            Cells: new Dictionary<string, string>
            {
                ["studentName"] = name,
                ["studentId"] = studentId,
                ["risk"] = risk,
                ["level"] = level.ToString(),
                ["lastActive"] = lastActive,
                ["predictionTime"] = predictionTime,
                ["counselor"] = counselor,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Assign", "#", "bi-person-plus", "btn btn-sm btn-outline-success")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildStudentSampleRows()
        => new[]
        {
            CreateStudentRow("2023-04102", "Ayesha Rahman", "CSE", "ayesha.rahman@aust.edu", "High", "11 Jul 2026", "Active"),
            CreateStudentRow("2023-03814", "Tanvir Ahmed", "EEE", "tanvir.ahmed@aust.edu", "Medium", "10 Jul 2026", "Active"),
            CreateStudentRow("2022-02751", "Nusrat Jahan", "BBA", "nusrat.jahan@aust.edu", "Low", "09 Jul 2026", "Disabled")
        };

    private static AdminSectionRowDto CreateStudentRow(string studentId, string name, string department, string email, string risk, string lastLogin, string status)
        => new(
            Id: studentId,
            PrimaryLabel: name,
            SecondaryLabel: email,
            BadgeText: risk,
            BadgeClass: $"badge {RiskBadge(ParseRisk(risk))}",
            Cells: new Dictionary<string, string>
            {
                ["studentId"] = studentId,
                ["photo"] = name.Substring(0, 1),
                ["name"] = name,
                ["department"] = department,
                ["email"] = email,
                ["risk"] = risk,
                ["lastLogin"] = lastLogin,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Edit", "#", "bi-pencil", "btn btn-sm btn-outline-secondary"),
                new AdminSectionActionDto("Disable", "#", "bi-slash-circle", "btn btn-sm btn-outline-danger"),
                new AdminSectionActionDto("Assign", "#", "bi-person-check", "btn btn-sm btn-outline-success")
            });

    private static RiskLevel ParseRisk(string risk)
        => risk switch
        {
            "High" => RiskLevel.High,
            "Medium" or "Moderate" => RiskLevel.Moderate,
            _ => RiskLevel.Low
        };

    private static IReadOnlyList<AdminSectionRowDto> BuildCounselorSampleRows()
        => new[]
        {
            CreateCounselorRow("Dr. M. Hasan", "Available", "42", "118", "12", "18m"),
            CreateCounselorRow("Dr. N. Akter", "Available", "35", "91", "9", "22m"),
            CreateCounselorRow("Dr. S. Rahman", "Busy", "27", "104", "4", "31m")
        };

    private static AdminSectionRowDto CreateCounselorRow(string name, string availability, string cases, string sessions, string bookings, string response)
        => new(
            Id: name,
            PrimaryLabel: name,
            SecondaryLabel: availability,
            BadgeText: availability,
            BadgeClass: availability == "Available" ? "badge text-bg-success" : "badge text-bg-warning",
            Cells: new Dictionary<string, string>
            {
                ["name"] = name,
                ["availability"] = availability,
                ["cases"] = cases,
                ["sessions"] = sessions,
                ["bookings"] = bookings,
                ["response"] = response,
                ["status"] = availability
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildVolunteerSampleRows()
        => new[]
        {
            CreateVolunteerRow("Saima Akter", "14", "62", "8", true),
            CreateVolunteerRow("Rakib Hossain", "9", "47", "5", true),
            CreateVolunteerRow("Mahi Chowdhury", "6", "21", "2", false)
        };

    private static AdminSectionRowDto CreateVolunteerRow(string name, string assigned, string forum, string resolved, bool active)
        => new(
            Id: name,
            PrimaryLabel: name,
            SecondaryLabel: active ? "Active" : "Idle",
            BadgeText: active ? "Active" : "Idle",
            BadgeClass: active ? "badge text-bg-success" : "badge text-bg-secondary",
            Cells: new Dictionary<string, string>
            {
                ["name"] = name,
                ["assigned"] = assigned,
                ["forum"] = forum,
                ["resolved"] = resolved,
                ["status"] = active ? "Active" : "Idle"
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildUserSampleRows()
        => new[]
        {
            CreateUserRow("System Admin", "Admin", "rashid.cse.20230104102@aust.edu", "11 Jul 2026", "Active"),
            CreateUserRow("Dr. M. Hasan", "Counselor", "m.hasan@aust.edu", "10 Jul 2026", "Active"),
            CreateUserRow("Saima Akter", "Volunteer", "saima.akter@aust.edu", "09 Jul 2026", "Active")
        };

    private static AdminSectionRowDto CreateUserRow(string name, string role, string email, string lastLogin, string status)
        => new(
            Id: email,
            PrimaryLabel: name,
            SecondaryLabel: role,
            BadgeText: status,
            BadgeClass: "badge text-bg-secondary",
            Cells: new Dictionary<string, string>
            {
                ["name"] = name,
                ["role"] = role,
                ["email"] = email,
                ["lastLogin"] = lastLogin,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Edit", "#", "bi-pencil", "btn btn-sm btn-outline-secondary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildForumSampleRows()
        => new[]
        {
            CreateForumRow("Need help with finals", "Student A", "3", "Pending"),
            CreateForumRow("Finding study group", "Student B", "2", "Reviewed"),
            CreateForumRow("Report on spam links", "Student C", "4", "Escalated")
        };

    private static AdminSectionRowDto CreateForumRow(string post, string reportedBy, string flags, string status)
        => new(
            Id: post,
            PrimaryLabel: post,
            SecondaryLabel: reportedBy,
            BadgeText: status,
            BadgeClass: "badge text-bg-warning",
            Cells: new Dictionary<string, string>
            {
                ["post"] = post,
                ["reportedBy"] = reportedBy,
                ["flags"] = flags,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Approve", "#", "bi-check-lg", "btn btn-sm btn-outline-success"),
                new AdminSectionActionDto("Reject", "#", "bi-x-lg", "btn btn-sm btn-outline-danger"),
                new AdminSectionActionDto("Delete", "#", "bi-trash", "btn btn-sm btn-outline-secondary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildBookingSampleRows()
        => new[]
        {
            CreateBookingRow("Ayesha Rahman", "Dr. M. Hasan", "Yes", "No", "No", "No", "11 Jul 2026 10:00"),
            CreateBookingRow("Tanvir Ahmed", "Dr. N. Akter", "No", "Yes", "No", "No", "11 Jul 2026 13:30"),
            CreateBookingRow("Nusrat Jahan", "Dr. S. Rahman", "No", "No", "No", "Yes", "12 Jul 2026 09:00")
        };

    private static AdminSectionRowDto CreateBookingRow(string student, string counselor, string pending, string approved, string rejected, string completed, string scheduled)
        => new(
            Id: student,
            PrimaryLabel: student,
            SecondaryLabel: counselor,
            BadgeText: approved == "Yes" ? "Approved" : pending == "Yes" ? "Pending" : completed == "Yes" ? "Completed" : "Rejected",
            BadgeClass: approved == "Yes" ? "badge text-bg-success" : pending == "Yes" ? "badge text-bg-warning" : completed == "Yes" ? "badge text-bg-secondary" : "badge text-bg-danger",
            Cells: new Dictionary<string, string>
            {
                ["student"] = student,
                ["counselor"] = counselor,
                ["pending"] = pending,
                ["approved"] = approved,
                ["rejected"] = rejected,
                ["completed"] = completed,
                ["scheduled"] = scheduled
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Open", "#", "bi-eye", "btn btn-sm btn-outline-primary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildNotificationSampleRows()
        => new[]
        {
            CreateNotificationRow("Risk alert escalated", "Ayesha Rahman moved to high risk", "Risk", "Unread"),
            CreateNotificationRow("Booking accepted", "Dr. M. Hasan accepted a booking", "Booking", "Read"),
            CreateNotificationRow("Forum report received", "Moderation queue has a new report", "Forum", "Unread")
        };

    private static AdminSectionRowDto CreateNotificationRow(string title, string message, string type, string status)
        => new(
            Id: title,
            PrimaryLabel: title,
            SecondaryLabel: message,
            BadgeText: status,
            BadgeClass: status == "Unread" ? "badge text-bg-primary" : "badge text-bg-secondary",
            Cells: new Dictionary<string, string>
            {
                ["title"] = title,
                ["message"] = message,
                ["type"] = type,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("View", "#", "bi-eye", "btn btn-sm btn-outline-primary"),
                new AdminSectionActionDto("Delete", "#", "bi-trash", "btn btn-sm btn-outline-danger")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildAuditSampleRows()
        => new[]
        {
            CreateAuditRow("Admin login", "ApplicationUser", "1", "11 Jul 2026 09:10"),
            CreateAuditRow("Prediction generated", "RiskScore", "28", "11 Jul 2026 09:21"),
            CreateAuditRow("Booking approved", "CounselorBooking", "85", "11 Jul 2026 09:42")
        };

    private static AdminSectionRowDto CreateAuditRow(string action, string entity, string entityId, string timestamp)
        => new(
            Id: $"{action}-{entityId}",
            PrimaryLabel: action,
            SecondaryLabel: entity,
            BadgeText: "Audit",
            BadgeClass: "badge text-bg-dark",
            Cells: new Dictionary<string, string>
            {
                ["user"] = "System",
                ["entity"] = entity,
                ["entityId"] = entityId,
                ["timestamp"] = timestamp
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Inspect", "#", "bi-search", "btn btn-sm btn-outline-primary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildReportSampleRows()
        => new[]
        {
            CreateReportRow("Risk Report", "Campus-wide", "PDF", "Ready"),
            CreateReportRow("Booking Report", "Weekly", "PDF", "Queued")
        };

    private static AdminSectionRowDto CreateReportRow(string report, string scope, string format, string status)
        => new(
            Id: report,
            PrimaryLabel: report,
            SecondaryLabel: scope,
            BadgeText: status,
            BadgeClass: status == "Ready" ? "badge text-bg-success" : "badge text-bg-warning",
            Cells: new Dictionary<string, string>
            {
                ["report"] = report,
                ["scope"] = scope,
                ["format"] = format,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Generate", "#", "bi-file-earmark-pdf", "btn btn-sm btn-outline-primary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildAnalyticsSampleRows()
        => new[]
        {
            CreateAnalyticsRow("Risk index", "18", "21", "-3"),
            CreateAnalyticsRow("Forum activity", "148", "129", "+19")
        };

    private static AdminSectionRowDto CreateAnalyticsRow(string metric, string current, string previous, string trend)
        => new(
            Id: metric,
            PrimaryLabel: metric,
            SecondaryLabel: current,
            BadgeText: trend,
            BadgeClass: trend.StartsWith('+') ? "badge text-bg-success" : "badge text-bg-danger",
            Cells: new Dictionary<string, string>
            {
                ["metric"] = metric,
                ["current"] = current,
                ["previous"] = previous,
                ["trend"] = trend
            },
            Actions: Array.Empty<AdminSectionActionDto>());

    private static IReadOnlyList<AdminSectionRowDto> BuildMlSampleRows()
        => new[]
        {
            CreateMlRow("risk-model.zip", "93.4%", "428", "Healthy"),
            CreateMlRow("forum-model.zip", "88.8%", "73", "Attention")
        };


    private static AdminSectionRowDto CreateMlRow(string model, string accuracy, string predictions, string health)
        => new(
            Id: model,
            PrimaryLabel: model,
            SecondaryLabel: health,
            BadgeText: health,
            BadgeClass: health == "Healthy" ? "badge text-bg-success" : "badge text-bg-warning",
            Cells: new Dictionary<string, string>
            {
                ["model"] = model,
                ["accuracy"] = accuracy,
                ["predictions"] = predictions,
                ["health"] = health
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Retrain", "#", "bi-arrow-repeat", "btn btn-sm btn-outline-primary")
            });

    private static IReadOnlyList<AdminSectionRowDto> BuildJobSampleRows()
        => new[]
        {
            CreateJobRow("Booking reminders", "Every 15 min", "11 Jul 2026 09:45", "Running"),
            CreateJobRow("Forum moderation sweep", "Hourly", "11 Jul 2026 09:00", "Failed")
        };


    private static AdminSectionRowDto CreateJobRow(string job, string schedule, string lastRun, string status)
        => new(
            Id: job,
            PrimaryLabel: job,
            SecondaryLabel: schedule,
            BadgeText: status,
            BadgeClass: status == "Completed" ? "badge text-bg-success" : status == "Running" ? "badge text-bg-primary" : "badge text-bg-danger",
            Cells: new Dictionary<string, string>
            {
                ["job"] = job,
                ["schedule"] = schedule,
                ["lastRun"] = lastRun,
                ["status"] = status
            },
            Actions: new[]
            {
                new AdminSectionActionDto("Open", "#", "bi-eye", "btn btn-sm btn-outline-primary")
            });

    private static string RiskBadge(RiskLevel level)
        => level switch
        {
            RiskLevel.High => "text-bg-danger",
            RiskLevel.Moderate => "text-bg-warning",
            _ => "text-bg-success"
        };

    private static string BookingBadge(BookingStatus status)
        => status switch
        {
            BookingStatus.Confirmed => "text-bg-success",
            BookingStatus.Cancelled or BookingStatus.NoShow => "text-bg-danger",
            BookingStatus.Completed => "text-bg-secondary",
            _ => "text-bg-warning"
        };
}
