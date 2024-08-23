using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Friends;

public record FriendRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid FriendId { get; set; }
}