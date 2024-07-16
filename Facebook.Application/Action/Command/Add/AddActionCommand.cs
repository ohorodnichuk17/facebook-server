using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Action.Command.Add;

public record AddActionCommand (string Name, string Emoji) : IRequest<ErrorOr<Guid>>;
