using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetAll;

public class GetAllFriendsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllFriendsQuery, ErrorOr<IEnumerable<UserEntity>>>
{
    public async Task<ErrorOr<IEnumerable<UserEntity>>> Handle(GetAllFriendsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var friends = await unitOfWork.User.GetAllFriendsAsync(request.UserId.ToString());

            if (friends.Count == 0)
            {
                return Error.NotFound("Friends not found");
            }

            return friends;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Error while retrieving friends: {ex.Message}");
        }
    }
}