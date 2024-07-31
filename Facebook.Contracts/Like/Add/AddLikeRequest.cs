using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Like.Add;

public class AddLikeRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid PostId { get; set; }
}
