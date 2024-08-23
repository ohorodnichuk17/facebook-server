using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetRelationshipsStatus;

public class GetRelationshipsStatusQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<GetRelationshipsStatusQuery, ErrorOr<RelationshipsStatus>>
{
    public async Task<ErrorOr<RelationshipsStatus>> Handle(GetRelationshipsStatusQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetCurrentUserId();

        var friendRequestResult = await unitOfWork.User.GetFriendRequestByFriendIdsAsync(currentUserId, request.FriendId);

        if (friendRequestResult.IsError)
        {
            return RelationshipsStatus.None;
        }

        var friendRequest = friendRequestResult.Value;

        if (friendRequest.IsAccepted)
        {
            return RelationshipsStatus.Friends;
        }

        if (friendRequest.SenderId.ToString() == currentUserId)
        {
            return RelationshipsStatus.UserWaitAccept;
        }

        return RelationshipsStatus.SendRequestToUser;
    }
}
