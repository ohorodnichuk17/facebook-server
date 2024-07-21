using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.Action.Add;

public record AddActionRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Name { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Emoji { get; set; }
}
