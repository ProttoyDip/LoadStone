namespace Lodestone.Domain.Common;

/// <summary>Auditable entity that is flagged as deleted rather than removed from the store.</summary>
public abstract class SoftDeleteEntity : AuditableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public string? DeletedBy { get; set; }
}
