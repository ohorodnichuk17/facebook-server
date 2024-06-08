using ErrorOr;
using MediatR;

namespace Facebook.Application.Post.Command.Delete;

public record DeletePostCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;