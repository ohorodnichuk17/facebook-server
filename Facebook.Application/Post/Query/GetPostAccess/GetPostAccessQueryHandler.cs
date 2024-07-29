using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Constants.ContentVisibility;
using Facebook.Domain.TypeExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetPostAccess;

public class GetPostAccessQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPostAccessQuery, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(GetPostAccessQuery request, CancellationToken cancellationToken)
    {
        var viewer = await unitOfWork.User.GetUserByIdAsync(request.ViewerId);
        var post = await unitOfWork.Post.GetByIdAsync(request.PostId);
        var friend = await unitOfWork.User.GetFriendByIdAsync(post.Value.UserId, viewer.Value.Id);

        if (post.IsError)
        {
            return post.Errors;
        }

        if (viewer.IsError)
        {
            return viewer.Errors;
        }

        switch (post.Value.Visibility)
        {
            case ContentVisibility.Public:
                return true;
            case ContentVisibility.Private:
                return post.Value.UserId == viewer.Value.Id;
            case ContentVisibility.FriendsOnly:
                if (friend.IsSuccess() || post.Value.UserId == viewer.Value.Id)
                {
                    return true;
                }
                return false;
            case ContentVisibility.FriendsExcept:
                if (friend.IsSuccess() && !post.Value.ExcludedFriends.Contains(viewer.Value.Id) || post.Value.UserId == viewer.Value.Id)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
}
