using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

public class ForumCategory : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<ForumPost> Posts { get; set; } = new List<ForumPost>();
}
