using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RejectFriendRequest;

public class RejectFriendRequestCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<RejectFriendRequestCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = currentUserService.GetCurrentUserId();
            var result =
                await unitOfWork.User.RejectFriendRequestAsync(currentUserId,
                    request.FriendId.ToString());

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}