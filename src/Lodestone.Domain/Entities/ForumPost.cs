using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

public class ForumPost : SoftDeleteEntity
{
    public int ForumCategoryId { get; set; }
    public ForumCategory? Category { get; set; }

    public string AuthorUserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public ForumPostStatus Status { get; set; } = ForumPostStatus.Published;

    public ICollection<ForumComment> Comments { get; set; } = new List<ForumComment>();
    public ICollection<ForumFlag> Flags { get; set; } = new List<ForumFlag>();
}
