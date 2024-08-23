using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Query.GetById;

public record GetReactionByIdQuery(Guid Id) : IRequest<ErrorOr<ReactionEntity>>;
