namespace Facebook.Domain.User;

public class FriendRequestEntity
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public UserEntity Sender { get; set; }
    public Guid ReceiverId { get; set; }
    public UserEntity Receiver { get; set; }
    private DateTime _sentAt;
    public DateTime SentAt
    {
        get { return _sentAt; }
        set
        {
            _sentAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
    public bool IsAccepted { get; set; } = false;
}