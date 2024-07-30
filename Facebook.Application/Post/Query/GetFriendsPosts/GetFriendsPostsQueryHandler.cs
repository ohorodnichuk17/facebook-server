using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetFriendsPosts;

public class GetFriendsPostsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetFriendsPostsQuery, ErrorOr<IEnumerable<PostEntity>>>
{
    public async Task<ErrorOr<IEnumerable<PostEntity>>> Handle(GetFriendsPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Post.GetFriendsPostsAsync(request.UserId);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving friends` posts: {ex.Message}");
            return Error.Failure($"Error while receiving friends` posts: {ex.Message}");
        }
    }
}
