using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

/// <summary>A hotline / support resource surfaced on the crisis page.</summary>
public class CrisisResource : SoftDeleteEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Url { get; set; }
    public bool IsEmergency { get; set; }
    public int DisplayOrder { get; set; }
}
