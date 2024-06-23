using Facebook.Domain.User;
using MediatR;
using ErrorOr;

namespace Facebook.Application.User.Friends.Query.GetById;

public record GetFriendByIdQuery(
    Guid UserId,
    Guid FriendId
) : IRequest<ErrorOr<UserEntity>>;