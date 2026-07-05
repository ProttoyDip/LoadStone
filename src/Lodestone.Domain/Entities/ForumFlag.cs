using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

/// <summary>A moderation flag raised against a post by a user or automated job.</summary>
public class ForumFlag : AuditableEntity
{
    public int ForumPostId { get; set; }
    public ForumPost? Post { get; set; }

    public string RaisedByUserId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public bool IsReviewed { get; set; }
}
