using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Query.GetById;

public class GetCommentByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCommentByIdQuery, ErrorOr<CommentEntity>>
{
    public async Task<ErrorOr<CommentEntity>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Comment.GetByIdAsync(request.Id);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving comment by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving comment by id {request.Id}: {ex.Message}");
        }
    }
}
