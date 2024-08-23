using Facebook.Domain.User;

namespace Facebook.Domain.Post;

public class ReactionEntity
{
    public Guid Id { get; set; }
    public string TypeCode { get; set; }
    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set
        {
            _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
    public PostEntity PostEntity { get; set; }
    public Guid PostId { get; set; }
    public UserEntity UserEntity { get; set; }
    public Guid UserId { get; set; }
}
