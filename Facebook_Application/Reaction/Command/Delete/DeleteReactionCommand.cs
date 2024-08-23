using ErrorOr;
using MediatR;

namespace Facebook.Application.Reaction.Command.Delete;

public record DeleteReactionCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;