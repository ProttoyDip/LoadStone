using System.ComponentModel.DataAnnotations;

namespace Lodestone.Web.ViewModels.Auth;

public class ForgotPasswordViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}
