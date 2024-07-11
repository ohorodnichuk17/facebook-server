using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Add;

public class AddCommentCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IPostRepository postRepository)
    : IRequestHandler<AddCommentCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId.ToString());
        var post = await postRepository.GetPostByIdAsync(request.PostId);

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
