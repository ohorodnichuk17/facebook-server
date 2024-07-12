using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetLikeByPostId;

public record GetLikesByPostIdQuery(Guid PostId) : IRequest<ErrorOr<IEnumerable<LikeEntity>>>;
