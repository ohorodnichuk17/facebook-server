using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Comment.Command.Add;

public class AddCommentCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddCommentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetUserByIdAsync(request.UserId.ToString());
        var post = await unitOfWork.Post.GetPostByIdAsync(request.PostId);

        if (user.IsError || post.IsError)
        {
            return Error.NotFound();
        }

        var commentEntity = new CommentEntity
        {
            Message = request.Message,
            UserId = request.UserId,
            PostId = request.PostId,
            CreatedAt = DateTime.Now,
        };

        var commentResult = await unitOfWork.Comment.CreateAsync(commentEntity);

        if (commentResult.IsError)
        {
            return commentResult.Errors;
        }

        var res = await unitOfWork.Comment.SaveAsync(commentEntity);

        if (res.IsError)
        {
            return res.Errors;
        }

        return res;
    }
}
