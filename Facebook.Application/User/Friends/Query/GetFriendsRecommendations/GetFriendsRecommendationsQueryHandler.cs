using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Application.User.Friends.Query.GetFriendsRecommendations;

public class GetFriendsRecommendationsQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    UserManager<UserEntity> userManager) 
    : IRequestHandler<GetFriendsRecommendationsQuery, ErrorOr<IEnumerable<UserEntity>>>
{
    public async Task<ErrorOr<IEnumerable<UserEntity>>> Handle(GetFriendsRecommendationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allUsers = await unitOfWork.User.GetAllUsersAsync();
            if (allUsers.IsError || allUsers.Value.Count == 0)
            {
                return Error.NotFound("No users found for recommendations");
            }

            string currentUserId = currentUserService.GetCurrentUserId();

            var friends = await unitOfWork.User.GetAllFriendsAsync(currentUserId);
            var friendsIds = friends.Select(f => f.Id.ToString()).ToHashSet();

            var random = new Random();

            var filteredUsers = allUsers.Value
                .Where(u => u.Id.ToString() != currentUserId && !friendsIds.Contains(u.Id.ToString()))
                .ToList();

            var nonAdminUsers = new List<UserEntity>();

            foreach (var user in filteredUsers)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (!roles.Contains("admin"))
                {
                    nonAdminUsers.Add(user);
                }
            }

            var recommendations = nonAdminUsers
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
