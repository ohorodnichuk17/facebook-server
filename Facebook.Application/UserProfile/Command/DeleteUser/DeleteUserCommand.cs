using ErrorOr;
using MediatR;

namespace Facebook.Application.UserProfile.Command.DeleteUser;

public record DeleteUserCommand(
    Guid UserId
) : IRequest<ErrorOr<bool>>;
