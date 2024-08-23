using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetReactionByPostId;

public record GetReactionsByPostIdQuery(Guid PostId) : IRequest<ErrorOr<IEnumerable<ReactionEntity>>>;
