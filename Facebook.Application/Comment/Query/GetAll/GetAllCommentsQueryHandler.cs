using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Comment.Query.GetAll;

public class GetAllCommentsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCommentsQuery, ErrorOr<IEnumerable<CommentEntity>>>
{
    public async Task<ErrorOr<IEnumerable<CommentEntity>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Comment.GetAllAsync();

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
            Console.WriteLine($"Error while receiving comments: {ex.Message}");
            return Error.Failure($"Error while receiving comments: {ex.Message}");
        }
    }
}
