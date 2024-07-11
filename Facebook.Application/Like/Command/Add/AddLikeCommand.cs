using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Like.Command.Add;

public record AddLikeCommand(
    Guid UserId,
    Guid PostId
) : IRequest<ErrorOr<Unit>>;
