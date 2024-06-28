using ErrorOr;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Post.Query.GetAll;

public class GetAllPostsQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetAllPostsQuery, ErrorOr<IEnumerable<PostEntity>>>
{
    public async Task<ErrorOr<IEnumerable<PostEntity>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await postRepository.GetAllPostsAsync();

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
            Console.WriteLine($"Error while receiving posts: {ex.Message}");
            return Error.Failure($"Error while receiving posts: {ex.Message}");
        }
    }
}
