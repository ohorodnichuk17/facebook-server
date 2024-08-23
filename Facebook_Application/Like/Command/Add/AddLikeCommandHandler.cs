using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Like.Command.Add;

public class AddLikeCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    : IRequestHandler<AddLikeCommand, ErrorOr<LikeEntity>>
{
    public async Task<ErrorOr<LikeEntity>> Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var user = await unitOfWork.User.GetUserByIdAsync(currentUserId);
        var post = await unitOfWork.Post.GetPostByIdAsync(request.PostId);

        if (user.IsError)
        {
            return user.Errors;
        }

        if (post.IsError)
        {
            return post.Errors;
        }

        var likeEntity = new LikeEntity
        {
            UserId = new Guid(currentUserId),
            PostId = request.PostId,
            isLiked = true
        };

        var result = await unitOfWork.Like.SaveIfNotExist(likeEntity);

        return result;
    }
}
