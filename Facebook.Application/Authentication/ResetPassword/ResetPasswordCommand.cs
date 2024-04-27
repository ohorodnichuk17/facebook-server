using Facebook.Domain.UserEntity;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Authentication.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string Token,
    string Password,
    string ConfirmPassword,
    string BaseUrl) : IRequest<ErrorOr<UserEntity>>;