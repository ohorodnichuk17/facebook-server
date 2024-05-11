using System.ComponentModel.DataAnnotations;
using Facebook.Contracts.Authentication.Common.Validation;
using Microsoft.AspNetCore.Http;

namespace Facebook.Contracts.Story.Create;

public record CreateStoryRequest
{
    [StringLength(1000, ErrorMessage = "{PropertyName} cannot exceed 1000 characters")]
    public string? Content { get; init; }
    
    [FileSize(2 * 1024 * 1024)]
    public IFormFile? Image { get; init; }
    
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required Guid UserId { get; init; }
}