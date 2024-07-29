using Facebook.Application.Common.Interfaces.Action.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;

namespace Facebook.Infrastructure.Repositories;

public class ActionRepository(FacebookDbContext context) : Repository<ActionEntity>(context), IActionRepository
{
}
