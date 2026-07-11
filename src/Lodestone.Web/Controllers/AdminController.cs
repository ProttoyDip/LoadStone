using Lodestone.Application.DTOs.Admin;
using Lodestone.Web.ViewModels.Admin;
using Lodestone.Application.Interfaces;
using Lodestone.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize(Roles = RoleConstants.Admin)]
public class AdminController : Controller
{
    private readonly IAdminDashboardService _adminDashboardService;

    public AdminController(IAdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        await SetShellAsync(cancellationToken);
        ViewData["AdminActiveSection"] = AdminSectionType.Dashboard.ToString();
        ViewData["Title"] = "Admin Dashboard";

        var model = new AdminDashboardViewModel(await _adminDashboardService.GetDashboardAsync(cancellationToken));
        return View(model);
    }

    public Task<IActionResult> Students(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Students, cancellationToken);
    public Task<IActionResult> Counselors(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Counselors, cancellationToken);
    public Task<IActionResult> Volunteers(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Volunteers, cancellationToken);
    public Task<IActionResult> Users(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Users, cancellationToken);
    public Task<IActionResult> RiskMonitoring(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.RiskMonitoring, cancellationToken);
    public Task<IActionResult> ForumModeration(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.ForumModeration, cancellationToken);
    public Task<IActionResult> CounselorBookings(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.CounselorBookings, cancellationToken);
    public Task<IActionResult> MoodJournals(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.MoodJournals, cancellationToken);
    public Task<IActionResult> Reports(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Reports, cancellationToken);
    public Task<IActionResult> Analytics(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Analytics, cancellationToken);
    public Task<IActionResult> MachineLearning(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.MachineLearning, cancellationToken);
    public Task<IActionResult> BackgroundJobs(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.BackgroundJobs, cancellationToken);
    public Task<IActionResult> Notifications(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Notifications, cancellationToken);
    public Task<IActionResult> AuditLogs(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.AuditLogs, cancellationToken);
    public Task<IActionResult> Settings(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Settings, cancellationToken);
    public Task<IActionResult> Profile(CancellationToken cancellationToken) => RenderSectionAsync(AdminSectionType.Profile, cancellationToken);

    private async Task<IActionResult> RenderSectionAsync(AdminSectionType section, CancellationToken cancellationToken)
    {
        await SetShellAsync(cancellationToken);
        ViewData["AdminActiveSection"] = section.ToString();
        ViewData["Title"] = GetTitle(section);

        var model = new AdminSectionViewModel(await _adminDashboardService.GetSectionAsync(section, cancellationToken));
        return View("Section", model);
    }

    private async Task SetShellAsync(CancellationToken cancellationToken)
    {
        ViewData["AdminShell"] = await _adminDashboardService.GetShellAsync(cancellationToken);
    }

    private static string GetTitle(AdminSectionType section)
        => section switch
        {
            AdminSectionType.Students => "Students",
            AdminSectionType.Counselors => "Counselors",
            AdminSectionType.Volunteers => "Volunteers",
            AdminSectionType.Users => "Users",
            AdminSectionType.RiskMonitoring => "Risk Monitoring",
            AdminSectionType.ForumModeration => "Forum Moderation",
            AdminSectionType.CounselorBookings => "Counselor Bookings",
            AdminSectionType.MoodJournals => "Mood Journals",
            AdminSectionType.Reports => "Reports",
            AdminSectionType.Analytics => "Analytics",
            AdminSectionType.MachineLearning => "Machine Learning",
            AdminSectionType.BackgroundJobs => "Background Jobs",
            AdminSectionType.Notifications => "Notifications",
            AdminSectionType.AuditLogs => "Audit Logs",
            AdminSectionType.Settings => "Settings",
            AdminSectionType.Profile => "Profile",
            _ => "Dashboard"
        };
}
