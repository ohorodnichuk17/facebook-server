using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Command.AddReplyComment;

public record AddReplyCommentCommand(Guid ParentId, string Message, Guid UserId, Guid PostId) : IRequest<ErrorOr<CommentEntity>>;