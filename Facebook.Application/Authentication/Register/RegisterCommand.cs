using MediatR;
using ErrorOr;
using Facebook.Application.Authentication.Common;

namespace Facebook.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    DateTime Birthday,
    string Gender,
    string BaseUrl
    ) : IRequest<ErrorOr<AuthenticationResult>>;