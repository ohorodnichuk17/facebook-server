using Microsoft.AspNetCore.Identity;

namespace Facebook.Domain.User;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public string Gender { get; set; }
    public bool IsBlocked { get; set; }

    public string? CoverPhoto { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Biography { get; set; }
    public bool IsProfilePublic { get; set; }
}