using ErrorOr;
using MediatR;

namespace Facebook.Application.Comment.Command.Add;

public record AddCommentCommand(
    string Message,
    Guid PostId
) : IRequest<ErrorOr<Unit>>;
