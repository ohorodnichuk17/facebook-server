using ErrorOr;
using Facebook.Application.Common.Interfaces.Comment.IRepository;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MapsterMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Facebook.Application.Comment.Command.AddReplyComment;

public class AddReplyCommentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddReplyCommentCommand, ErrorOr<CommentEntity>>
{
    public async Task<ErrorOr<CommentEntity>> Handle(AddReplyCommentCommand request, CancellationToken cancellationToken)
    {
        var parentComment = await unitOfWork.Comment.GetByIdAsync(request.ParentId);
        if (parentComment.IsError)
        {
            throw new ArgumentException("Parent comment not found");
        }

        var reply = new CommentEntity
        {
            Message = request.Message,
            UserId = request.UserId,
            PostId = request.PostId,
            ParentCommentId = request.ParentId,
            CreatedAt = DateTime.UtcNow
        };

        var commentResult = await unitOfWork.Comment.CreateAsync(reply);

        if (commentResult.IsError)
        {
            return commentResult.Errors;
        }

        return reply;
    }
}
