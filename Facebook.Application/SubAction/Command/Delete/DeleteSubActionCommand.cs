using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Command.Delete;

public record DeleteSubActionCommand(Guid Id) : IRequest<ErrorOr<bool>>;
