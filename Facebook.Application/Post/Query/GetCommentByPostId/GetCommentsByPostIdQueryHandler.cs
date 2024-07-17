using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.GetCommentByPostId;

public class GetCommentsByPostIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCommentsByPostIdQuery, ErrorOr<IEnumerable<CommentEntity>>>
{
    public async Task<ErrorOr<IEnumerable<CommentEntity>>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Comment.GetCommentsByPostIdAsync(request.PostId);

            if (result.IsError)
            {
                return Error.Failure(result.Errors.ToString() ?? string.Empty);
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while receiving comments: {ex.Message}");
            return Error.Failure($"Error while receiving comments: {ex.Message}");
        }
    }
}
