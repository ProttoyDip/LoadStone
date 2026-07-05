using Lodestone.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize]
public class StudentController : Controller
{
    private readonly IActivityLogService _activityLogService;

    public StudentController(IActivityLogService activityLogService)
        => _activityLogService = activityLogService;

    public IActionResult Index() => View();
    public IActionResult Details(int id) => View();
}
