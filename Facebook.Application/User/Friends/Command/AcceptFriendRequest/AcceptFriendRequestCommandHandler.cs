using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.User.Friends.Command.AcceptFriendRequest;

public class AcceptFriendRequestCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<AcceptFriendRequestCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.User
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