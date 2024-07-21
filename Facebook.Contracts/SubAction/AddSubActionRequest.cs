using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.SubAction;

public record AddSubActionRequest
{
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public string Name { get; set; }
    [Required(ErrorMessage = "{PropertyName} must not be empty")]
    public Guid ActionId { get; set; }
}
