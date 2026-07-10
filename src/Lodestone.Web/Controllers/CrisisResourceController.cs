using Lodestone.Application.Interfaces;
using Lodestone.Web.ViewModels.CrisisResource;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

/// <summary>Public crisis resources page (no auth so anyone in distress can reach it).</summary>
public class CrisisResourceController : Controller
{
    private readonly ICrisisResourceService _crisisResourceService;

    public CrisisResourceController(ICrisisResourceService crisisResourceService)
        => _crisisResourceService = crisisResourceService;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var resources = await _crisisResourceService.GetActiveResourcesAsync(cancellationToken);
        var model = new CrisisResourceViewModel
        {
            EmergencyResources = resources.Where(resource => resource.IsEmergency).ToArray(),
            SupportResources = resources.Where(resource => !resource.IsEmergency).ToArray()
        };

        return View(model);
    }
}
