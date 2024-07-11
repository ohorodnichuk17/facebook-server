using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Delete;

public record DeleteCommentCommand(
    Guid Id    
) : IRequest<ErrorOr<bool>>;
