using Facebook.Domain.Constants.ContentVisibility;
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
    public Guid? ActionId { get; set; }
    public ActionEntity? Action { get; set; }
    public Guid? SubActionId { get; set; }
    public SubActionEntity? SubAction { get; set; }
    public ICollection<ReactionEntity>? Reactions { get; set; }
    public ICollection<LikeEntity>? Likes { get; set; }
    public ICollection<CommentEntity>? Comments { get; set; }
    public bool? IsArchive { get; set; } = false;

    private DateTime _createdAt;
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set { _createdAt = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid? FeelingId { get; set; }
    public FeelingEntity? Feeling { get; set; }

    public string Visibility { get; set; }
    public IList<Guid>? ExcludedFriends { get; set; }
}