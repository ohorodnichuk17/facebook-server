using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Friends;

public record GetAllFriendRequestsRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }
}
