using System.ComponentModel.DataAnnotations;
using Facebook.Contracts.Authentication.Common.Validation;
using Microsoft.AspNetCore.Http;

namespace Facebook.Contracts.Post.Create;

public record CreatePostRequest
{
    [StringLength(10, ErrorMessage = "{PropertyName} cannot exceed 10 characters")]
    public string? Title { get; init; }

    [StringLength(3000, ErrorMessage = "{PropertyName} cannot exceed 3000 characters")]
    public string? Content { get; init; }

    [MaxLength(10, ErrorMessage = "You can specify up to 10 tags only")]
    public List<string>? Tags { get; init; }

    [StringLength(100, ErrorMessage = "{PropertyName} cannot exceed 100 characters")]
    public string? Location { get; init; }

    [FileSize(2 * 1024 * 1024, ErrorMessage = "Each image must not exceed 2MB")]
    public List<IFormFile>? Images { get; init; }

    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public required Guid UserId { get; init; }

    [Required(ErrorMessage = "{PropertyName} must be specified")]
    public bool IsArchive { get; init; }
}