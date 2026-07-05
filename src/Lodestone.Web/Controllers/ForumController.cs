using Lodestone.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize]
public class ForumController : Controller
{
    private readonly IForumService _forumService;

    public ForumController(IForumService forumService) => _forumService = forumService;

    public IActionResult Index() => View();
    public IActionResult Category(int id) => View();
    public IActionResult Post(int id) => View();
}
