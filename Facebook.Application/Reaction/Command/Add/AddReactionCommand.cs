using ErrorOr;
using MediatR;

namespace Facebook.Application.Reaction.Command.Add;

public record AddReactionCommand(
    string Emoji,
    Guid PostId
) : IRequest<ErrorOr<Unit>>;
