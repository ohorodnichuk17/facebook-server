using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Comment.AddReplyComment;

public class AddReplyCommentRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid ParentId { get; set; }
    [Required]
    public required string Message { get; set; }
}
