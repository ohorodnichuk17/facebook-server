using MediatR;
using ErrorOr;
using Facebook.Domain.User;

namespace Facebook.Application.Authentication.ChangeEmail;

public record ChangeEmailCommand(
    string UserId,
    string Email,
    string Token,
    string BaseUrl) : IRequest<ErrorOr<UserEntity>>;