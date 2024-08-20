using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RejectFriendRequest;

public record RejectFriendRequestCommand(
    Guid FriendId
) : IRequest<ErrorOr<Unit>>;