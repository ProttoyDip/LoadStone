namespace Lodestone.Domain.Common;

/// <summary>Base type for all persisted entities with a surrogate key.</summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}
