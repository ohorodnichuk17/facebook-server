using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.Comment.Edit;

public class EditCommentRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid Id { get; set; }
    [Required]
    public string Message { get; set; }
}
