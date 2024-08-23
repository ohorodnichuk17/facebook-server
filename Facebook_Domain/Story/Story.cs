using Facebook.Domain.User;

namespace Facebook.Domain.Story;

public class StoryEntity
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set { _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    public DateTime ExpiresAt => CreatedAt.AddHours(24);

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}