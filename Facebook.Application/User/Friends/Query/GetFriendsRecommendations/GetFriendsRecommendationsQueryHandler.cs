using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
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

            var allUsers = await unitOfWork.User.GetAllUsersAsync();
            var friends = await unitOfWork.User.GetAllFriendsAsync(currentUserId);

            if (allUsers.IsError || allUsers.Value.Count == 0)
            {
                return Error.NotFound("No users found for recommendations");
            }

            var friendIds = friends.Select(f => f.Id.ToString()).ToHashSet();

            var random = new Random();

            var recommendations = allUsers.Value
                .Where(u => u.Id.ToString() != currentUserId)
                .Where(u => !friendIds.Contains(u.Id.ToString()))
                .OrderBy(u => random.Next())
                .Take(20)
                .ToList();

            return recommendations;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Error while retrieving users: {ex.Message}");
        }
    }
}