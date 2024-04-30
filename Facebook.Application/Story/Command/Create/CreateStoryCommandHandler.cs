using ErrorOr;
using Facebook.Application.Common.Interfaces.Services;
using Facebook.Application.Common.Interfaces.Story.IService;
using Facebook.Domain.Story;
using Facebook.Domain.TypeExtensions;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, ErrorOr<StoryEntity>>
{
    private readonly IStoryService _storyService;
    private readonly IImageStorageService _imageStorageService;

    public CreateStoryCommandHandler(IStoryService storyService, IImageStorageService imageStorageService)
    {
        _storyService = storyService;
        _imageStorageService = imageStorageService;
    }

    public async Task<ErrorOr<StoryEntity>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        var storyEntity = new StoryEntity
        {
            Id = request.Id,
            Content = request.Content,
            CreatedAt = DateTime.Now 
        };

        var storyResult = await _storyService.CreateStoryAsync(storyEntity);

        // if (result.IsSuccess())
        // {
        //     return result.Value;
        // }
        // else
        // {
        //     return Error.Failure("Error");
        // }
        if (storyResult.IsError)
        {
            return storyResult.Errors;
        }

        if (request.Image != null)
        {
            var imageName = await _imageStorageService.AddStoryImageAsync(request.Image);
            if (imageName == null)
            {
                return Error.Unexpected("Avatar saving error");
            }

            storyEntity.Image = imageName;
        }

        var imageResult = await _storyService.SaveStoryAsync(storyEntity);

        if (imageResult.IsError)
        {
            return imageResult.Errors;
        }

        return imageResult;
    }
}