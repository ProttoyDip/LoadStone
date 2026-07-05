using Lodestone.Application.Interfaces;
using Lodestone.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize(Policy = PolicyConstants.CanAccessAdmin)]
public class AdminController : Controller
{
    private readonly IReportService _reportService;

    public AdminController(IReportService reportService) => _reportService = reportService;

    public IActionResult Index() => View();
    public IActionResult Users() => View();
    public IActionResult Reports() => View();
}
