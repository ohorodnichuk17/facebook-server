namespace Facebook.Application.DTO;

public class LikeDto
{
    public Guid Id { get; set; }
    public bool isLiked { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}
