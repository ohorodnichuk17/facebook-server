using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Domain.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Action.IRepository;

public interface IActionRepository : IRepository<ActionEntity>
{
}
