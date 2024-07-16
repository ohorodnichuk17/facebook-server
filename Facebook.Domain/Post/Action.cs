using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.Post;

public class ActionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Emoji { get; set; }
    public ICollection<SubActionEntity> SubActions { get; set; }
}
