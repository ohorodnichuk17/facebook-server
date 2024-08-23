namespace Facebook.Contracts.Chat;

public class CreateChatRequest
{
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
}
