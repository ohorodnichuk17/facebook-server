using ErrorOr;
using MediatR;

namespace Facebook.Application.Chat.Command.Create;

public record CreateChatCommand(string UserId, string FriendId) : IRequest<ErrorOr<Guid>>;
