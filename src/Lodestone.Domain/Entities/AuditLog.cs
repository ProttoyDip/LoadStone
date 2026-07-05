using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

/// <summary>Immutable audit trail of security- and privacy-relevant actions.</summary>
public class AuditLog : BaseEntity
{
    public string? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? EntityName { get; set; }
    public string? EntityId { get; set; }
    public string? Details { get; set; }
    public DateTime TimestampUtc { get; set; }
}
