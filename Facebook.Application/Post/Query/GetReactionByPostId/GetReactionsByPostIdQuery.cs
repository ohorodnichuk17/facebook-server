using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetReactionByPostId;

public record GetReactionsByPostIdQuery(Guid PostId) : IRequest<ErrorOr<IEnumerable<ReactionEntity>>>;
