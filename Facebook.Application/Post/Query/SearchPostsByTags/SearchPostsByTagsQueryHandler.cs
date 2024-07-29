using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.SearchPostsByTags;

public class SearchPostsByTagsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SearchPostsByTagsQuery, ErrorOr<List<PostEntity>>>
{
    public async Task<ErrorOr<List<PostEntity>>> Handle(SearchPostsByTagsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var posts = await unitOfWork.Post.SearchPostsByTags(request.Tag);
            
            if(posts.IsError)
            {
                return posts.Errors;
            }

            return posts;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}