using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RemoveFriend;

public class RemoveFriendCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RemoveFriendCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userRepository.RemoveFriendAsync(request.UserId.ToString(), request.FriendId.ToString());
            
            if (result.IsError)
            {
                return result.Errors;
            }
        
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}