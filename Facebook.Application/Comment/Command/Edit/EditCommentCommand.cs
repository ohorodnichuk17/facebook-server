using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.Edit;

public record EditCommentCommand(string Message, Guid Id) : IRequest<ErrorOr<Unit>>;