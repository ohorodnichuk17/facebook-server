using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Command.Delete;

public record DeleteMessageByIdCommand(Guid Id) : IRequest<ErrorOr<bool>>;