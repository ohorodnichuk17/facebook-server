using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Like.Query.GetAll;

public class GetAllLikesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllLikesQuery, ErrorOr<IEnumerable<LikeEntity>>>
{
    public async Task<ErrorOr<IEnumerable<LikeEntity>>> Handle(GetAllLikesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Like.GetAllAsync();

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
            Console.WriteLine($"Error while receiving likes: {ex.Message}");
            return Error.Failure($"Error while receiving likes: {ex.Message}");
        }
    }
}
