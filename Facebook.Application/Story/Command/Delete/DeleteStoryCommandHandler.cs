using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;

namespace Facebook.Application.Story.Command.Delete;

public class DeleteStoryCommandHandler(IStoryRepository storyRepository)
    : IRequestHandler<DeleteStoryCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await storyRepository.DeleteStoryAsync(request.Id);
        
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