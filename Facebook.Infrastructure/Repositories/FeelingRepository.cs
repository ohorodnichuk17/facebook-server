using ErrorOr;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Feeling;

public class FeelingRepository(FacebookDbContext context) 
    : Repository<FeelingEntity>(context), IFeelingRepository
{
}