using Facebook.Domain.User;

namespace Facebook.Domain.Chat;

public class MessageEntity
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set
        {
            _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
    public Guid ChatId { get; set; }
    public ChatEntity Chat { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}
