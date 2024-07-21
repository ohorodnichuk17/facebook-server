using MediatR;
using ErrorOr;

namespace Facebook.Application.Admin.Command.BlockUser;

public record BlockUserCommand(string UserId) : IRequest<ErrorOr<Unit>>;