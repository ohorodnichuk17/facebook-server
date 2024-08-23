using ErrorOr;
using MediatR;

namespace Facebook.Application.SubAction.Command.Add;

public record AddSubActionCommand(string Name, Guid ActionId) : IRequest<ErrorOr<Guid>>;
