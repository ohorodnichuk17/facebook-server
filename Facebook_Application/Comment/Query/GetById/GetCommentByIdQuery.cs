using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Query.GetById;

public record GetCommentByIdQuery(Guid Id) : IRequest<ErrorOr<CommentEntity>>;
