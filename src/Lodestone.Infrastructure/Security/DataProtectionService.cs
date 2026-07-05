using Microsoft.AspNetCore.DataProtection;

namespace Lodestone.Infrastructure.Security;

/// <summary>Protects sensitive fields (e.g. journal notes) using ASP.NET Data Protection.</summary>
public class DataProtectionService
{
    private readonly IDataProtector _protector;

    public DataProtectionService(IDataProtectionProvider provider)
        => _protector = provider.CreateProtector("Lodestone.Sensitive");

    public string Protect(string plaintext) => _protector.Protect(plaintext);
    public string Unprotect(string ciphertext) => _protector.Unprotect(ciphertext);
}
