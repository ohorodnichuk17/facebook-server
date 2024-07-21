using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Reaction.Command.Add;

public class AddReactionCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddReactionCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetUserByIdAsync(request.UserId.ToString());
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
            Id = Guid.NewGuid(),
            UserId = user.Value.Id,
            PostId = post.Value.Id,
            CreatedAt = DateTime.Now,
            TypeCode = request.TypeCode
        };

        var reactionResult = await unitOfWork.Reaction.CreateAsync(reaction);

        if (reactionResult.IsError)
        {
            return reactionResult.Errors;
        }

        return Unit.Value;
    }
}
