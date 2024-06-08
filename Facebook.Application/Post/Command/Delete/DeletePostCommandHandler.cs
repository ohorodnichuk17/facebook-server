using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Post.IRepository;

namespace Facebook.Application.Post.Command.Delete;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, ErrorOr<bool>>
{
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postRepository.DeletePostAsync(request.Id);

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