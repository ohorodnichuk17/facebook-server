using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetCommentByPostId;

public record GetCommentsByPostIdQuery(Guid PostId) : IRequest<ErrorOr<IEnumerable<CommentEntity>>>;
