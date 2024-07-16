using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.SubAction.Query.GetAll;

public record GetAllSubActionsQuery() : IRequest<ErrorOr<IEnumerable<SubActionEntity>>>;