using MediatR;
using ErrorOr;
using Facebook.Application.Authentication.Common;
using Facebook.Domain.UserEntity;

namespace Facebook.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password
    ) : IRequest<ErrorOr<AuthenticationResult>>;