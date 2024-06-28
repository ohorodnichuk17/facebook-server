using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetById;

public record GetPostByIdQuery(Guid Id) : IRequest<ErrorOr<PostEntity>>;