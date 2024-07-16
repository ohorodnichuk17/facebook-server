using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using MediatR;

namespace Facebook.Application.User.Friends.Command.AcceptFriendRequest;

public class AcceptFriendRequestCommandHandler(
    IUserRepository userRepository) : IRequestHandler<AcceptFriendRequestCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userRepository
                .AcceptFriendRequestAsync(request.UserId.ToString(), request.FriendId.ToString());

            if (result.IsError)
            {
                return result.Errors;
            }

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}