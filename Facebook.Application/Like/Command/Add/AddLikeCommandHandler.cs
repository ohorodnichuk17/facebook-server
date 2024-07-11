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

namespace Facebook.Application.Like.Command.Add;

public class AddLikeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IPostRepository postRepository)
    : IRequestHandler<AddLikeCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId.ToString());
        var post = await postRepository.GetPostByIdAsync(request.PostId);

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
            UserId = request.UserId,
            PostId = request.PostId,
            isLiked = true
        };

        var likeResult = await unitOfWork.Like.CreateAsync(likeEntity);

        if (likeResult.IsError)
        {
            return likeResult.Errors;
        }

        var res = await unitOfWork.Like.SaveAsync(likeEntity);

        if (res.IsError)
        {
            return res.Errors;
        }

        return res;
    }
}
