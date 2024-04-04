using MediatR;
using ErrorOr;
using Facebook.Domain.User;

namespace Facebook.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password
    ) : IRequest<ErrorOr<string>>;