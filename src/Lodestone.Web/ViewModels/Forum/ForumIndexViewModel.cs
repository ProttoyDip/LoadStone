using Lodestone.Application.DTOs.Forum;

namespace Lodestone.Web.ViewModels.Forum;

public class ForumIndexViewModel
{
    public IReadOnlyList<ForumCategoryDto> Categories { get; set; } = new List<ForumCategoryDto>();
}
