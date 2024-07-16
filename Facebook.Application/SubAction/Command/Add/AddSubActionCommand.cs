using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Command.Add;

public record AddSubActionCommand(string Name, Guid ActionId) : IRequest<ErrorOr<Guid>>;
