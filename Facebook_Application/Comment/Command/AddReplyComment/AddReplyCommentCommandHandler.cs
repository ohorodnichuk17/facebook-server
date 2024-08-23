using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Comment.Command.AddReplyComment;

public class AddReplyCommentCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<AddReplyCommentCommand, ErrorOr<CommentEntity>>
{
    public async Task<ErrorOr<CommentEntity>> Handle(AddReplyCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var parentComment = await unitOfWork.Comment.GetByIdAsync(request.ParentId);
        if (parentComment.IsError)
        {
            throw new ArgumentException("Parent comment not found");
        }

        var reply = new CommentEntity
        {
            Message = request.Message,
            UserId = new Guid(currentUserId),
            PostId = parentComment.Value.PostId,
            ParentCommentId = request.ParentId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await unitOfWork.Comment.SaveAsync(reply);

        return result;
    }
}
