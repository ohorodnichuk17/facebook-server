using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetAll;

public record GetAllFriendsQuery(Guid UserId) : IRequest<ErrorOr<IEnumerable<UserEntity>>>;