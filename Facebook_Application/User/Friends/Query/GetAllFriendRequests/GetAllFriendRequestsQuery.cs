using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetAllFriendRequests;

public record GetAllFriendRequestsQuery(Guid UserId) : IRequest<ErrorOr<IEnumerable<UserEntity>>>;
