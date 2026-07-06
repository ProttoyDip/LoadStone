using System.ComponentModel.DataAnnotations;

namespace Lodestone.Web.ViewModels.Auth;

public class ResetPasswordViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8,
        ErrorMessage = "The {0} must be at least {2} characters long.")]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Confirm password")]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Round-tripped through the reset link (hidden fields on the form).
    [Required] public string Token { get; set; } = string.Empty;
}
