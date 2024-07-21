using ErrorOr;
using MediatR;

namespace Facebook.Application.Admin.Command.BanUser;

public record BanUserCommand(string UserId) : IRequest<ErrorOr<Unit>>;