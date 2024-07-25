using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Command.Edit;

public record EditMessageCommand(string Content, Guid Id) : IRequest<ErrorOr<Unit>>;