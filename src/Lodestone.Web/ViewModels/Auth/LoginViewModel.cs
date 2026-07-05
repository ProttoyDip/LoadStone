using System.ComponentModel.DataAnnotations;

namespace Lodestone.Web.ViewModels.Auth;

public class LoginViewModel
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}
