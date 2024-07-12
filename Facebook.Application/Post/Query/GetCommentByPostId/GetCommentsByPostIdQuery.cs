using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetCommentByPostId;

public record GetCommentsByPostIdQuery(Guid PostId) : IRequest<ErrorOr<IEnumerable<CommentEntity>>>;
