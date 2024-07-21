using ErrorOr;
using MediatR;

namespace Facebook.Application.Reaction.Command.Add;

public record AddReactionCommand(
    string TypeCode,
    Guid PostId,
    Guid UserId
) : IRequest<ErrorOr<Unit>>;
