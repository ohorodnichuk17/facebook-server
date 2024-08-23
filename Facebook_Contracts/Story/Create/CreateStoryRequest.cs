using Facebook.Contracts.Authentication.Common.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Story.Create;

public record CreateStoryRequest
{
    [StringLength(1000, ErrorMessage = "{PropertyName} cannot exceed 1000 characters")]
    public string? Content { get; init; }

    [FileSize(2 * 1024 * 1024)]
    public IFormFile? Image { get; init; }
}