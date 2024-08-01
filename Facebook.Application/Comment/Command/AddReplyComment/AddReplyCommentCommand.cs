using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Comment.Command.AddReplyComment;

public record AddReplyCommentCommand(Guid ParentId, string Message) : IRequest<ErrorOr<CommentEntity>>;