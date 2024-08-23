using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Like.Query.GetById;

public class GetLikeByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetLikeByIdQuery, ErrorOr<LikeEntity>>
{
    public async Task<ErrorOr<LikeEntity>> Handle(GetLikeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Like.GetByIdAsync(request.Id);

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
            Console.WriteLine($"Error retrieving like by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving like by id {request.Id}: {ex.Message}");
        }
    }
}
