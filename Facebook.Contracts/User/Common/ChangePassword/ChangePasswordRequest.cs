using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.User.Common.ChangePassword;

public record ChangePasswordRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    [Length(8, 24)]
    public required string CurrentPassword { get; init; }

    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    [Length(8, 24)]
    public required string NewPassword { get; init; }

    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    [Compare("NewPassword",
        ErrorMessage = "The password and confirmation password do not match.")]
    [Length(8, 24)]
    public required string ConfirmNewPassword { get; init; }
}