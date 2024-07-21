using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Domain.Post;

public class SubActionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ActionEntity Action { get; set; }
    public Guid ActionId { get; set; }
}
