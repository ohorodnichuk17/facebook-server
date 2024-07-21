using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:Facebook.Application/Message/Command/Delete/DeleteMessageByIdCommand.cs
namespace Facebook.Application.Message.Command.Delete;

public record DeleteMessageByIdCommand(Guid Id) : IRequest<ErrorOr<bool>>;
========
namespace Facebook.Application.Action.Command.Delete;

public record DeleteActionCommand(Guid Id) : IRequest<ErrorOr<bool>>;
>>>>>>>> 83d94b6a6ee7e0792f18ac90786866f6166cdd20:Facebook.Application/Action/Command/Delete/DeleteActionCommand.cs
