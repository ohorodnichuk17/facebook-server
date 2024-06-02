using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Application.Common.Interfaces.Services;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, ErrorOr<Unit>>
{
    private readonly IStoryRepository _storyRepository;
    private readonly IImageStorageService _imageStorageService;
    private readonly IUserRepository _userRepository;

    public CreateStoryCommandHandler(IStoryRepository storyRepository, IImageStorageService imageStorageService, IUserRepository userRepository)
    {
        _storyRepository = storyRepository;
        _imageStorageService = imageStorageService;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        // var user = await _userRepository.GetUserByIdAsync(request.UserId.ToString()); 
        //
        // if (user.IsError)
        // {
        //     return user.Errors;
        // }
        //
        // var userResult = user.Value;
        
        var storyEntity = new StoryEntity
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,
            UserId = request.UserId,
            // User = userResult
        };

        var storyResult = await _storyRepository.CreateStoryAsync(storyEntity);
        
        if (storyResult.IsError)
        {
            return storyResult.Errors;
        }

        if (request.Image != null)
        {
            var imageName = await imageStorageService.AddStoryImageAsync(request.Image);
            if (imageName == null)
            {
                return Error.Unexpected("Avatar saving error");
            }
        
            storyEntity.Image = imageName;
        }


        var result = await _storyRepository.SaveStoryAsync(storyEntity);
        
        if (result.IsError)
        {
            return result.Errors;
        }
        
        return result;
    }
}