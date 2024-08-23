using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RemoveFriend;

public record RemoveFriendCommand(Guid FriendId) : IRequest<ErrorOr<bool>>;