using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Admin.Base;

public record BaseAdminRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required string Id { get; init; }
}