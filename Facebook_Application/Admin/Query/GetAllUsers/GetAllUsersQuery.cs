using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Admin.Query.GetAllUsers;

public record GetAllUsersQuery() : IRequest<ErrorOr<List<UserEntity>>>;