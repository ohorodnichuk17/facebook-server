using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Command.AcceptFriendRequest;

public record AcceptFriendRequestCommand(
    Guid UserId,
    Guid FriendRequestId
) : IRequest<ErrorOr<Unit>>;