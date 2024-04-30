using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Story.IService;

namespace Facebook.Application.Story.Command.Delete;

public class DeleteStoryCommandHandler : IRequestHandler<DeleteStoryCommand, ErrorOr<bool>>
{
    private readonly IStoryService _storyService;

    public DeleteStoryCommandHandler(IStoryService storyService)
    {
        _storyService = storyService;
    }
    
    public async Task<ErrorOr<bool>> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _storyService.DeleteStoryAsync(request.Id);
            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}