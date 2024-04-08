using Facebook.Domain.User;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Authentication.ChangeEmail;

public record ChangeEmailCommand(
    string UserId,
    string Email,
    string Token) : IRequest<ErrorOr<User>>;