using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetAllFriendRequests;

public class GetAllFriendRequestsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllFriendRequestsQuery, ErrorOr<IEnumerable<UserEntity>>>
{
    public async Task<ErrorOr<IEnumerable<UserEntity>>> Handle(GetAllFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var friends = await unitOfWork.User.GetAllFriendRequestsAsync(request.UserId.ToString());

            if (friends.Value.Count == 0)
            {
                return new List<UserEntity>();
            }

            return friends.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Error while retrieving friends: {ex.Message}");
        }
    }
}