using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Chat.Command.Delete;

public record DeleteChatByIdCommand(Guid Id) : IRequest<ErrorOr<bool>>;