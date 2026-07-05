using Lodestone.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize(Roles = "Student")]
public class JournalController : Controller
{
    private readonly IJournalService _journalService;

    public JournalController(IJournalService journalService) => _journalService = journalService;

    public IActionResult Index() => View();
}
