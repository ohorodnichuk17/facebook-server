using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Friends;

public record AcceptFriendRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid FriendId { get; set; }
}