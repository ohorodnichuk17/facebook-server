using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RemoveFriend;

public class RemoveFriendCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<RemoveFriendCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = currentUserService.GetCurrentUserId();
            var result = await unitOfWork.User.RemoveFriendAsync(currentUserId, request.FriendId.ToString());

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