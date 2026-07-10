using Lodestone.Application.DTOs.Forum;

namespace Lodestone.Web.ViewModels.Forum;

public class ForumPostDetailViewModel
{
    public ForumPostDetailDto Post { get; set; } = null!;
    public CreateForumCommentDto NewComment { get; set; } = new(0, string.Empty);
}
