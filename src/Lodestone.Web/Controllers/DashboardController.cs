using Lodestone.Application.Interfaces;
using Lodestone.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

/// <summary>Renders analytics dashboards. Controllers call Application services only.</summary>
[Authorize]
public class DashboardController : Controller
{
    private readonly IRiskScoringService _riskScoringService;

    public DashboardController(IRiskScoringService riskScoringService)
        => _riskScoringService = riskScoringService;

    public IActionResult Index()
    {
        if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Counselor))
            return RedirectToAction("Index", "Student");

        return View();
    }
}
