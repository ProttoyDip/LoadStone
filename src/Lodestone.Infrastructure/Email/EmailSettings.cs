namespace Lodestone.Infrastructure.Email;

public class EmailSettings
{
    public const string SectionName = "Email";
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string FromAddress { get; set; } = "no-reply@lodestone.local";
    public string DisplayName { get; set; } = "Lodestone";
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
