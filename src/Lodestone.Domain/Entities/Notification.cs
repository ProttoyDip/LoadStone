using Lodestone.Domain.Common;
using Lodestone.Domain.Enums;

namespace Lodestone.Domain.Entities;

public class Notification : BaseEntity
{
    public string RecipientUserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
