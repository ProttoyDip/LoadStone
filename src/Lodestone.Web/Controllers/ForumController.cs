using Lodestone.Application.DTOs.Forum;
using Lodestone.Application.Interfaces;
using Lodestone.Web.ViewModels.Forum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodestone.Web.Controllers;

[Authorize]
public class ForumController : Controller
{
    private readonly IForumService _forumService;

    public ForumController(IForumService forumService) => _forumService = forumService;

    // GET /Forum — category index
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var categories = await _forumService.GetCategoriesAsync(cancellationToken);
        return View(new ForumIndexViewModel { Categories = categories });
    }

    // GET /Forum/Category/{id}
    public async Task<IActionResult> Category(int id, CancellationToken cancellationToken)
    {
        var posts = await _forumService.GetPostsAsync(id, cancellationToken);
        var vm = new ForumCategoryViewModel
        {
            CategoryId = id,
            Posts      = posts,
            NewPost    = new CreateForumPostDto(id, string.Empty, string.Empty),
        };
        return View(vm);
    }

    // GET /Forum/Post/{id}
    public async Task<IActionResult> Post(int id, CancellationToken cancellationToken)
    {
        var post = await _forumService.GetPostWithCommentsAsync(id, cancellationToken);
        if (post is null) return NotFound();
        return View(new ForumPostDetailViewModel
        {
            Post       = post,
            NewComment = new CreateForumCommentDto(id, string.Empty),
        });
    }

    // POST /Forum/CreatePost
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost(CreateForumPostDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Body))
        {
            ModelState.AddModelError(string.Empty, "Title and body are required.");
            var posts = await _forumService.GetPostsAsync(model.CategoryId, cancellationToken);
            return View("Category", new ForumCategoryViewModel
            {
                CategoryId = model.CategoryId,
                Posts      = posts,
                NewPost    = model,
            });
        }

        var created = await _forumService.CreatePostAsync(model, cancellationToken);
        return RedirectToAction(nameof(Post), new { id = created.Id });
    }

    // POST /Forum/AddComment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(CreateForumCommentDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Body))
        {
            return RedirectToAction(nameof(Post), new { id = model.PostId });
        }

        await _forumService.AddCommentAsync(model, cancellationToken);
        return RedirectToAction(nameof(Post), new { id = model.PostId });
    }

    // POST /Forum/Flag/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Flag(int id, string reason, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(reason))
            await _forumService.FlagPostAsync(id, reason, cancellationToken);

        TempData["Info"] = "Post has been flagged for review.";
        return RedirectToAction(nameof(Post), new { id });
    }
}
