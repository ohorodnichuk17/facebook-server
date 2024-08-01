using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MapsterMapper;
using MediatR;

namespace Facebook.Application.Comment.Command.Add;

public class AddCommentCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddCommentCommand, ErrorOr<CommentEntity>>
{
    public async Task<ErrorOr<CommentEntity>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var user = await unitOfWork.User.GetUserByIdAsync(currentUserId);
        var post = await unitOfWork.Post.GetPostByIdAsync(request.PostId);

        if (user.IsError || post.IsError)
        {
            return Error.NotFound();
        }

        var comment = mapper.Map<CommentEntity>(request);
        comment.UserId = new Guid(currentUserId);

        var commentResult = await unitOfWork.Comment.CreateAsync(comment);

        if (commentResult.IsError)
        {
            return commentResult.Errors;
        }

        var result = await unitOfWork.Comment.SaveAsync(comment);

        return result;
    }
}
