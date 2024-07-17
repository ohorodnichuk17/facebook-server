using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MapsterMapper;
using MediatR;

namespace Facebook.Application.Comment.Command.Add;

public class AddCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
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

        var comment = mapper.Map<CommentEntity>(request);

        var commentResult = await unitOfWork.Comment.CreateAsync(comment);

        if (commentResult.IsError)
        {
            return commentResult.Errors;
        }

        var res = await unitOfWork.Comment.SaveAsync(comment);

        if (res.IsError)
        {
            return res.Errors;
        }

        return res;
    }
}
