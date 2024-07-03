namespace Facebook.Domain.User;

public class UserProfileEntity
{
    public Guid Id { get; set; }
    public string? CoverPhoto { get; set; }
    public string? Biography { get; set; }
    public bool IsBlocked { get; set; } = false;
    public bool IsProfilePublic { get; set; } = true;
    public string? Pronouns { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public UserEntity UserEntity { get; set; }
    public Guid UserId { get; set; }
}
