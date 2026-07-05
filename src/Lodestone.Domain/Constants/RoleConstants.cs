namespace Lodestone.Domain.Constants;

/// <summary>Canonical Identity role names. Kept in Domain so all layers agree.</summary>
public static class RoleConstants
{
    public const string Student = "Student";
    public const string Volunteer = "Volunteer";
    public const string Counselor = "Counselor";
    public const string Admin = "Admin";

    public static readonly string[] All = { Student, Volunteer, Counselor, Admin };
}
