using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Command.Delete;

public record DeleteReactionCommand(
    Guid PostId,
    Guid UserId
) : IRequest<ErrorOr<bool>>;