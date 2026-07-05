namespace Lodestone.Infrastructure.Security;

/// <summary>Helpers for masking/anonymizing PII in logs and exports.</summary>
public static class PrivacyHelper
{
    public static string MaskEmail(string email)
    {
        var at = email.IndexOf('@');
        return at <= 1 ? "***" : $"{email[0]}***{email[at..]}";
    }
}
