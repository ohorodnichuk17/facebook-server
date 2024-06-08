using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Domain.Post;

namespace Facebook.Application.Post.Query.GetById;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ErrorOr<PostEntity>>
{
    private readonly IPostRepository _postRepository;

    public GetPostByIdQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<ErrorOr<PostEntity>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postRepository.GetPostByIdAsync(request.Id);

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
            Console.WriteLine($"Error retrieving post by id {request.Id}: {ex.Message}");
            return Error.Failure($"Error retrieving post by id {request.Id}: {ex.Message}");
        }
    }
}