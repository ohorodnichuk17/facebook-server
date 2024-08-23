using Facebook.Domain.User;

namespace Facebook.Domain.Post;

public class LikeEntity
{
    public Guid Id { get; set; }
    public bool isLiked { get; set; }
    public PostEntity PostEntity { get; set; }
    public Guid PostId { get; set; }
    public UserEntity UserEntity { get; set; }
    public Guid UserId { get; set; }
}
