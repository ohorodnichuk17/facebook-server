using ErrorOr;
using Facebook.Application.Common.Interfaces.Admin.IService;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetFriendsRecommendations;

public class GetFriendsRecommendationsQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetFriendsRecommendationsQuery, ErrorOr<IEnumerable<UserEntity>>>
{
    public async Task<ErrorOr<IEnumerable<UserEntity>>> Handle(GetFriendsRecommendationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            string currentUserId = currentUserService.GetCurrentUserId();

            var userFriends = await unitOfWork.User.GetAllFriendsAsync(currentUserId);

            var friends = await unitOfWork.User.GetAllUsersAsync();

            if (friends.IsError || friends.Value.Count == 0)
            {
                return Error.NotFound("Friends recommendation not found");
            }

            var users = friends.Value;

            var friendIds = userFriends.Select(f => f.Id.ToString()).ToHashSet();
            friendIds.Add(currentUserId);

            users = users.Where(u => !friendIds.Contains(u.Id.ToString())).ToList();

            var random = new Random();
            var recommendations = users.OrderBy(_ => random.Next()).Take(20).ToList();

            return recommendations;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Error while retrieving friends: {ex.Message}");
        }
    }
}
