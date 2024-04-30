using Microsoft.AspNetCore.Identity;

namespace Facebook.Domain.User;

public class UserEntity : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    private DateTime _birthday;
    public DateTime Birthday
    {
        get { return _birthday; }
        set
        {
            _birthday = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
    
    public string? Avatar { get; set; }

    public string Gender { get; set; }
}