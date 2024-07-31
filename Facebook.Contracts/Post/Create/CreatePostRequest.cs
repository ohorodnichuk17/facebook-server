using Facebook.Contracts.Authentication.Common.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
    public required string Visibility { get; init; }
    public List<Guid>? ExcludedFriends { get; init; }

    public bool? IsArchive { get; init; } = false;

    public Guid? FeelingId { get; init; }
    public Guid? ActionId { get; init; }
    public Guid? SubActionId { get; init; }
}