using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Comment.Command.Add;

public record AddCommentCommand(
    string Message,
    Guid PostId
) : IRequest<ErrorOr<CommentEntity>>;
