namespace Facebook.Application.DTO;

public class StoryDto
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt => CreatedAt.AddHours(24);
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public Guid UserId { get; set; }
}
