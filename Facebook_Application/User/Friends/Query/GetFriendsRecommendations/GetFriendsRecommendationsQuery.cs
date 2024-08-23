using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetFriendsRecommendations;

public record GetFriendsRecommendationsQuery() : IRequest<ErrorOr<IEnumerable<UserEntity>>>;
