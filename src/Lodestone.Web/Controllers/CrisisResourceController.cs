using Lodestone.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

/// <summary>Public crisis resources page (no auth so anyone in distress can reach it).</summary>
public class CrisisResourceController : Controller
{
    private readonly ICrisisResourceService _crisisResourceService;

    public CrisisResourceController(ICrisisResourceService crisisResourceService)
        => _crisisResourceService = crisisResourceService;

    public IActionResult Index() => View();
}
