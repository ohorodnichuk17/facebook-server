using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetById;

public class GetPostByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPostByIdQuery, ErrorOr<PostEntity>>
{
    public async Task<ErrorOr<PostEntity>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Post.GetByIdAsync(request.Id);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving post by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving post by id {request.Id}: {ex.Message}");
        }
    }
}