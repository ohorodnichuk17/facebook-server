using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Like.Add;

public class LikeRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid PostId { get; set; }
}
