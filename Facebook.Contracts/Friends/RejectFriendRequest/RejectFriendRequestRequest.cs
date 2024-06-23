using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Friends.RejectFriendRequest;

public record RejectFriendRequestRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid FriendRequestId { get; set; }
}