using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetAll;

public record GetAllPostsQuery : IRequest<ErrorOr<IEnumerable<PostEntity>>>;