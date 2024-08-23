using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetLikeByPostId;

public record GetLikesByPostIdQuery(Guid PostId) : IRequest<ErrorOr<IEnumerable<LikeEntity>>>;
