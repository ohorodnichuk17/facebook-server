using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.Post;

public class ReactionEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Emoji { get; set; }
    public ICollection<PostEntity> Posts { get; set; }  
}
