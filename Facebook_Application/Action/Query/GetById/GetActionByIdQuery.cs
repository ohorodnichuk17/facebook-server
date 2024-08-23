using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Action.Query.GetById;

public record GetActionByIdQuery(Guid Id) : IRequest<ErrorOr<ActionEntity>>;
