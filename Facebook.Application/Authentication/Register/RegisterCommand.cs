using ErrorOr;
using Facebook.Application.Authentication.Common;
using MediatR;

namespace Facebook.Application.Authentication.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    DateTime Birthday,
    string Gender,
    byte[]? Avatar,
    string? Role,
    string BaseUrl) : IRequest<ErrorOr<AuthenticationResult>>;