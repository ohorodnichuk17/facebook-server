using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Post.IRepository;

namespace Facebook.Application.Post.Command.Delete;

public class DeletePostCommandHandler(IPostRepository postRepository)
    : IRequestHandler<DeletePostCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await postRepository.DeletePostAsync(request.Id);

            if (result.IsError)
            {
                return result.Errors;
            }

            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}