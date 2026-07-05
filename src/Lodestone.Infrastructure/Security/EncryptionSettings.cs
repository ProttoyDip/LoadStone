namespace Lodestone.Infrastructure.Security;

public class EncryptionSettings
{
    public const string SectionName = "Encryption";
    public string KeyRingPath { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = "Lodestone";
}
