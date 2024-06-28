using ErrorOr;
using Facebook.Application.Authentication.Common;
using MediatR;

namespace Facebook.Application.Authentication.Login;

public record LoginQuery(
    string Email,
    string Password
    ) : IRequest<ErrorOr<AuthenticationResult>>;