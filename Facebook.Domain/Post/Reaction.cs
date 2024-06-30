using Facebook.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.Post;

public class ReactionEntity
{
    public Guid Id { get; set; }
    public string TypeCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public PostEntity PostEntity { get; set; }
    public Guid PostId { get; set; }
    public UserEntity UserEntity { get; set; }
    public Guid UserId { get; set; }
}
