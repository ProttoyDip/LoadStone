using Lodestone.Application.DTOs.Forum;

namespace Lodestone.Web.ViewModels.Forum;

public class ForumCategoryViewModel
{
    public int CategoryId { get; set; }
    public IReadOnlyList<ForumPostDto> Posts { get; set; } = new List<ForumPostDto>();
    public CreateForumPostDto NewPost { get; set; } = new(0, string.Empty, string.Empty);
}
