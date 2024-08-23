using Facebook.Domain.User;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Admin.Query.GetUserById;

public record GetUserByIdQuery(
    Guid Id) : IRequest<ErrorOr<UserEntity>>;