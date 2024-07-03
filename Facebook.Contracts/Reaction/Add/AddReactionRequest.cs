using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Contracts.Reaction.Add;

public class AddReactionRequest
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public string TypeCode { get; set; }

}
