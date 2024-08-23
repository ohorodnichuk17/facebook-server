using Facebook.Domain.User;

namespace Facebook.Domain.Chat;

public class ChatEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<MessageEntity> Messages { get; set; }
    public ICollection<UserEntity> Users { get; set; }
    public ICollection<ChatUserEntity> ChatUsers { get; set; }
}
