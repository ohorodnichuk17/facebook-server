using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Command.SendFriendRequest;

public record SendFriendRequestCommand(
    Guid UserId,
    Guid FriendId,
    string baseUrl
) : IRequest<ErrorOr<Unit>>;