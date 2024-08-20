using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Command.SendFriendRequest;

public record SendFriendRequestCommand(
    Guid FriendId,
    string baseUrl
) : IRequest<ErrorOr<Unit>>;