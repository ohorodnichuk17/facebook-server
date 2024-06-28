using System.ComponentModel.DataAnnotations;
using Facebook.Contracts.Story.Common.Validation;

namespace Facebook.Contracts.Story.Delete;

public record DeleteStoryRequest
{
    [Required(ErrorMessage = "{PropertyName} is required.")]
    [GuidValidation(ErrorMessage = "{PropertyName} must be a valid GUID.")]
    public required string Id { get; init; }
}