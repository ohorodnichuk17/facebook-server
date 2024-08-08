using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using MediatR;

namespace Facebook.Application.Like.Command.Add;

public class DeleteLikeByPostIdCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteLikeByPostIdCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteLikeByPostIdCommand request, CancellationToken cancellationToken)
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

        var result = await unitOfWork.Like.DeleteByPostId(user.Value.Id, post.Value.Id);

        return result;
    }
}
