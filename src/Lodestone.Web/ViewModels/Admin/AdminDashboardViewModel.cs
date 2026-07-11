using Lodestone.Application.DTOs.Admin;

namespace Lodestone.Web.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public AdminDashboardViewModel(AdminDashboardDto dashboard) => Dashboard = dashboard;

    public AdminDashboardDto Dashboard { get; }
}
