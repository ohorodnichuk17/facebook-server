using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetPostAccess;

public record GetPostAccessQuery(Guid ViewerId, Guid PostId) : IRequest<ErrorOr<bool>>;
