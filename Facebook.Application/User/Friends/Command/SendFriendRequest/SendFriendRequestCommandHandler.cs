using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using MediatR;

namespace Facebook.Application.User.Friends.Command.SendFriendRequest;

public class SendFriendRequestCommandHandler(
    IUserRepository userRepository) : IRequestHandler<SendFriendRequestCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userRepository
                .SendFriendRequestAsync(request.UserId.ToString(), request.FriendId.ToString());
            
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