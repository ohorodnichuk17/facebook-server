using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public class CreateStoryCommandHandler(
    IStoryRepository storyRepository,
    IImageStorageService imageStorageService,
    IUserRepository userRepository) : IRequestHandler<CreateStoryCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId.ToString());

        if (user.IsError)
        {
            return user.Errors;
        }

        var userResult = user.Value;

        var storyEntity = new StoryEntity
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,
            UserId = request.UserId,
        };

        var storyResult = await storyRepository.CreateStoryAsync(storyEntity);

        if (storyResult.IsError)
        {
            return storyResult.Errors;
        }

        if (request.Image != null)
        {
            var imageName = await imageStorageService.SaveImageAsByteArrayAsync(request.Image);
            if (imageName == null)
            {
                return Error.Unexpected("Avatar saving error");
            }
            storyEntity.Image = imageName;
        }

        var result = await storyRepository.SaveStoryAsync(storyEntity);

        if (result.IsError)
        {
            return result.Errors;
        }

        return result;
    }
}