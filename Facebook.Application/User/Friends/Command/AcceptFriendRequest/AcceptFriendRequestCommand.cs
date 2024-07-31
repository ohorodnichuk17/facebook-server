using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Command.AcceptFriendRequest;

public record AcceptFriendRequestCommand(
    Guid FriendId
) : IRequest<ErrorOr<Unit>>;