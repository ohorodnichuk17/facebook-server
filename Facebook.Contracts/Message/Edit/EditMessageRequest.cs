using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.Message.Edit;

public class EditMessageRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid Id { get; set; }
    [Required]
    public string Content { get; set; }
}
