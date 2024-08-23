using Facebook.Domain.User;

namespace Facebook.Application.DTO;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid PostId { get; set; }
    public UserEntity UserEntity { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentCommentId { get; set; }
}
