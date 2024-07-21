using ErrorOr;
using MediatR;

namespace Facebook.Application.Comment.Command.Delete;

public record DeleteCommentCommand(Guid Id) : IRequest<ErrorOr<bool>>
{
    public DeleteCommentCommand(string requestId) : this(Guid.Parse(requestId))
    {
    }
}
