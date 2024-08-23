using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.UserProfile.Query.GetPostsByUserId;

public record GetPostsByUserIdQuery(Guid UserId) : IRequest<ErrorOr<IEnumerable<PostEntity>>>;