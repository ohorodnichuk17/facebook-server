using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Query.GetAll;

public record GetAllReactionsQuery() : IRequest<ErrorOr<IEnumerable<ReactionEntity>>>;