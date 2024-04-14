using Facebook.Domain.UserEntity;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Authentication.ChangeEmail;

public record ChangeEmailCommand(
    string UserId,
    string Email,
    string Token) : IRequest<ErrorOr<UserEntity>>;