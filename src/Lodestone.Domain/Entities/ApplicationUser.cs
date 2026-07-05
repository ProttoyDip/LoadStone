using Microsoft.AspNetCore.Identity;

namespace Lodestone.Domain.Entities;

/// <summary>Identity user. Concrete role data lives in the profile entities below.</summary>
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastLoginUtc { get; set; }
    public bool IsActive { get; set; } = true;

    public StudentProfile? StudentProfile { get; set; }
    public CounselorProfile? CounselorProfile { get; set; }
    public VolunteerProfile? VolunteerProfile { get; set; }
}
