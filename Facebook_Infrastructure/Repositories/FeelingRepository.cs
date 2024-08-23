using Facebook.Application.Common.Interfaces.IRepository.Feeling;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;

namespace Facebook.Infrastructure.Repositories;

public class FeelingRepository(FacebookDbContext context) 
    : Repository<FeelingEntity>(context), IFeelingRepository
{
}