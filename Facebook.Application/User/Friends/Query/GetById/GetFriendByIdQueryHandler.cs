using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetById;

public class GetFriendByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetFriendByIdQuery, ErrorOr<UserEntity>>
{
    public async Task<ErrorOr<UserEntity>> Handle(GetFriendByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var friend = await userRepository
                .GetFriendByIdAsync(request.UserId.ToString(), request.FriendId.ToString());

            if (friend.IsError)
            {
                return friend.Errors;
            }

            return friend;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Error while retrieving friend: {ex.Message}");
        }
    }
}