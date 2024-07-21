using ErrorOr;
using Facebook.Application.Common.Interfaces.Action.IRepository;
using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Infrastructure.Repositories.Action;

public class SubActionRepository(FacebookDbContext context) : Repository<SubActionEntity>(context), ISubActionRepository
{
}
