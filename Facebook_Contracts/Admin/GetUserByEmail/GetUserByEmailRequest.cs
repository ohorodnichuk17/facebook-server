using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Admin.GetUserByEmail;

public record GetUserByEmailRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required string Email { get; set; }
}