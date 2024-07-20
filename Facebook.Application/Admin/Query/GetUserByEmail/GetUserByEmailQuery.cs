using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Admin.Query.GetUserByEmail;

public record GetUserByEmailQuery(
    string Email) : IRequest<ErrorOr<UserEntity>>;