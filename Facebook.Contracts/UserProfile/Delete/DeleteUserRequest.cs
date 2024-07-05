using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.UserProfile.Delete;

public record DeleteUserRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required string UserId { get; init; }
}