using Facebook.Domain.User;

namespace Facebook.Domain.Post;

public class PostEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public List<string>? Tags { get; set; }
    public string? Location { get; set; }
    public ICollection<ImagesEntity>? Images { get; set; }

    public bool IsArchive { get; set; }

    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set { _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}