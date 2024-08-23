using ErrorOr;
using MediatR;
namespace Facebook.Application.Action.Command.Delete;

public record DeleteActionCommand(Guid Id) : IRequest<ErrorOr<bool>>;
