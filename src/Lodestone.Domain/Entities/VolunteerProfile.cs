using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

public class VolunteerProfile : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public string? Bio { get; set; }
    public bool IsApproved { get; set; }
}
