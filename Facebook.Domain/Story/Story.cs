using System.ComponentModel.DataAnnotations.Schema;
using Facebook.Domain.User;

namespace Facebook.Domain.Story;

public class StoryEntity
{
    public Guid Id { get; set; }
    public string? Content { get; set; } 
    public string? Image { get; set; } 
    // public string? Video { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime ExpiresAt => CreatedAt.AddHours(24); 

    public bool IsExpired => DateTime.Now > ExpiresAt; 
    
    [ForeignKey("UserEntity")]
    public Guid UserId { get; set; } 
    public UserEntity User { get; set; }
}