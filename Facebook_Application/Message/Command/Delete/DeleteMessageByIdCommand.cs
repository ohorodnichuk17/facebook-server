using ErrorOr;
using MediatR;
namespace Facebook.Application.Message.Command.Delete;

public record DeleteMessageByIdCommand(Guid Id) : IRequest<ErrorOr<bool>>;
