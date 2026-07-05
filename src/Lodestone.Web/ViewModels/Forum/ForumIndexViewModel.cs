using Lodestone.Application.DTOs.Forum;

namespace Lodestone.Web.ViewModels.Forum;

public class ForumIndexViewModel
{
    public IReadOnlyList<ForumPostDto> Posts { get; set; } = new List<ForumPostDto>();
    public CreateForumPostDto NewPost { get; set; } = new(0, string.Empty, string.Empty);
}
