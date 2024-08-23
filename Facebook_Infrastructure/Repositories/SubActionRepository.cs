using Facebook.Application.Common.Interfaces.IRepository.Action;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;

namespace Facebook.Infrastructure.Repositories;

public class SubActionRepository(FacebookDbContext context) : Repository<SubActionEntity>(context), ISubActionRepository
{
}
