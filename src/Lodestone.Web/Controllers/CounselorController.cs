using Lodestone.Application.Interfaces;
using Lodestone.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize(Policy = PolicyConstants.CanViewRiskQueue)]
public class CounselorController : Controller
{
    private readonly ICounselorQueueService _queueService;

    public CounselorController(ICounselorQueueService queueService)
        => _queueService = queueService;

    public IActionResult Queue() => View();
    public IActionResult Reviews() => View();
}
