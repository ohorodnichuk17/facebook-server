using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Admin.BlockAndUnblockUser;

public record BlockAndUnblockUserRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required string UserId { get; init; }
}