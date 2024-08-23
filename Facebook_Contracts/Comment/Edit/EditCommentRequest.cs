using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.Comment.Edit;

public class EditCommentRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid Id { get; set; }
    [Required]
    public string Message { get; set; }
}
