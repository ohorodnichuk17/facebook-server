using ErrorOr;
using MediatR;

namespace Facebook.Application.Admin.Command.UnBlockUser;

public record UnBlockUserCommand(string UserId) : IRequest<ErrorOr<Unit>>;