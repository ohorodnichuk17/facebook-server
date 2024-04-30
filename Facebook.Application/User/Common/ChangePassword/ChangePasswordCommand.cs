using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Users.Common.ChangePassword;
public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword,
    string UserId) : IRequest<ErrorOr<UserEntity>>;