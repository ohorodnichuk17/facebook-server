using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Infrastructure.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Reaction;

public class ReactionRepository(FacebookDbContext context) : IReactionRepository
{

}
