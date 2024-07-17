using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.User.Friends.Command.RemoveFriend;

public class RemoveFriendCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveFriendCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.User.RemoveFriendAsync(request.UserId.ToString(), request.FriendId.ToString());

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