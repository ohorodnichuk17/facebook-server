using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RejectFriendRequest;

public class RejectFriendRequestCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<RejectFriendRequestCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await unitOfWork.User.RejectFriendRequestAsync(request.UserId.ToString(),
                    request.FriendRequestId.ToString());

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