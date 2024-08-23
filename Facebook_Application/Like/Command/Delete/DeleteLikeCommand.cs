using ErrorOr;
using MediatR;

namespace Facebook.Application.Like.Command.Delete;

public record DeleteLikeCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;
