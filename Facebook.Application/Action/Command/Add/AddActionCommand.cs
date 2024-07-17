using ErrorOr;
using MediatR;

namespace Facebook.Application.Action.Command.Add;

public record AddActionCommand(string Name, string Emoji) : IRequest<ErrorOr<Guid>>;
