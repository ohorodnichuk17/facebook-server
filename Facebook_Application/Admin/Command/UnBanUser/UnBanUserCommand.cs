using ErrorOr;
using MediatR;

namespace Facebook.Application.Admin.Command.UnBanUser;

public record UnBanUserCommand(string UserId) : IRequest<ErrorOr<Unit>>;