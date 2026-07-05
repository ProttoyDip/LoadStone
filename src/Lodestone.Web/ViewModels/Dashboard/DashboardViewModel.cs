using Lodestone.Application.DTOs.Dashboard;

namespace Lodestone.Web.ViewModels.Dashboard;

/// <summary>View model wrapping the Application dashboard DTO for rendering + Chart.js.</summary>
public class DashboardViewModel
{
    public DashboardSummaryDto? Summary { get; set; }
    public string RiskTrendJson { get; set; } = "[]";
}
