namespace Facebook.Domain.User;

public class UserProfileEntity
{
    public Guid Id { get; set; }
    public string? CoverPhoto { get; set; }
    public string? Biography { get; set; }
    private bool _isProfilePublic = true;
    public bool IsProfilePublic 
    { 
        get => _isProfilePublic;
        set => _isProfilePublic = value;
    }
    
    private bool _isBlocked = false;
    public bool IsBlocked 
    { 
        get => _isBlocked;
        set => _isBlocked = value;
    }
    public string? Pronouns { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public UserEntity UserEntity { get; set; }
    public Guid UserId { get; set; }
}
