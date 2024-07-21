using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:Facebook.Application/Action/Command/Delete/DeleteActionCommand.cs
namespace Facebook.Application.Action.Command.Delete;

public record DeleteActionCommand(Guid Id) : IRequest<ErrorOr<bool>>;
========
namespace Facebook.Application.Message.Command.Delete;

public record DeleteMessageByIdCommand(Guid Id) : IRequest<ErrorOr<bool>>;
>>>>>>>> 4111a05fb8094745fac06fb59c0811667cf3a9e1:Facebook.Application/Message/Command/Delete/DeleteMessageByIdCommand.cs
