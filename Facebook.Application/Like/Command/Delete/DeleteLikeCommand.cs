using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Like.Command.Delete;

public record DeleteLikeCommand(
    Guid Id    
) : IRequest<ErrorOr<bool>>;
