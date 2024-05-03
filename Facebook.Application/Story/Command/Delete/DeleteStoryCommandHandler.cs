using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IRepository;

namespace Facebook.Application.Story.Command.Delete;

public class DeleteStoryCommandHandler : IRequestHandler<DeleteStoryCommand, ErrorOr<bool>>
{
    private readonly IStoryRepository _storyRepository;

    public DeleteStoryCommandHandler(IStoryRepository storyRepository)
    {
        _storyRepository = storyRepository;
    }
    
    public async Task<ErrorOr<bool>> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storyRepository.DeleteStoryAsync(request.Id);
        
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