using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

public class ForumComment : SoftDeleteEntity
{
    public int ForumPostId { get; set; }
    public ForumPost? Post { get; set; }

    public string AuthorUserId { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public ForumPostStatus Status { get; set; } = ForumPostStatus.Published;
}
