using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Authentication.ResendConfirmEmail;

public record ResendConfirmEmailRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    [EmailAddress(ErrorMessage = "{PropertyName} is not a valid email address")]
    public required string Email { get; init; }
}