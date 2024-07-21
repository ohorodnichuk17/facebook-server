using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetAll;

public class GetAllPostsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllPostsQuery, ErrorOr<IEnumerable<PostEntity>>>
{
    public async Task<ErrorOr<IEnumerable<PostEntity>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Post.GetAllAsync();

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving posts: {ex.Message}");
            return Error.Failure($"Error while receiving posts: {ex.Message}");
        }
    }
}
