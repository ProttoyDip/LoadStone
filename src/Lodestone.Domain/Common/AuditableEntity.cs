namespace Lodestone.Domain.Common;

/// <summary>Adds creation/modification audit stamps. Set by the persistence layer.</summary>
public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public string? ModifiedBy { get; set; }
}
