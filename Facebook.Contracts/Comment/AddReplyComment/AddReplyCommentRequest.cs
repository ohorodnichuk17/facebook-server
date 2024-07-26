using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.Comment.AddReplyComment;

public class AddReplyCommentRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid ParentId { get; set; }
    [Required]
    public string Message { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid PostId { get; set; }
}
