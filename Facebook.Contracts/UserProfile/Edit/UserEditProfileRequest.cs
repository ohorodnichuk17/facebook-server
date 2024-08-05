using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Facebook.Contracts.UserProfile.Edit;

public record UserEditProfileRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? CoverPhoto { get; set; }
    public IFormFile? Avatar { get; set; }
    public string? Country { get; set; }
    public string? Pronouns { get; set; }
    public string? Region { get; set; }
    public string? Biography { get; set; }
    public bool? isOnline { get; set; }
    public bool? IsProfilePublic { get; set; } = true; 
    public bool? IsBlocked { get; set; } = false;
}
