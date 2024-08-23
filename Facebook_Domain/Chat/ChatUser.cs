using Facebook.Domain.User;

namespace Facebook.Domain.Chat;

public class ChatUserEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid ChatId { get; set; }
    public ChatEntity Chat { get; set; }
}
