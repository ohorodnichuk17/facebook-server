using Facebook.Application.Common.Interfaces.Action.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Action;

public class ActionRepository(FacebookDbContext context) : Repository<ActionEntity>(context), IActionRepository
{
}
