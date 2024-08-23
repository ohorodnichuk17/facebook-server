using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Comment.Add;

public class AddCommentRequest
{
    [Required]
    public required string Message { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid PostId { get; set; }
}
