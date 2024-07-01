using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.UserProfile.EditUserProfile;

public class UserEditProfileRequest
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? CoverPhoto { get; set; }
    public IFormFile? Avatar { get; set; }
    public string? Country { get; set; }
    public string? Pronouns { get; set; }
    // public string? City { get; set; }
    public string? Region { get; set; }
    public string? Biography { get; set; }
    public bool IsProfilePublic { get; set; }
    public bool IsBlocked { get; set; }
}
