using Facebook.Domain.User;

namespace Facebook.Application.DTO;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}
