using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Reaction.Command.Add;

public class AddReactionCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddReactionCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
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

        var reaction = new ReactionEntity
        {
            UserId = user.Value.Id,
            PostId = post.Value.Id,
            CreatedAt = DateTime.Now,
            TypeCode = request.Emoji
        };

        var reactionResult = await unitOfWork.Reaction.CreateAsync(reaction);

        if (reactionResult.IsError)
        {
            return reactionResult.Errors;
        }

        return Unit.Value;
    }
}
