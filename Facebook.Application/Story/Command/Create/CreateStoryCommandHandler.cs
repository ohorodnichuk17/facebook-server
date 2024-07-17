using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Story;
using MapsterMapper;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public class CreateStoryCommandHandler(
    IUnitOfWork unitOfWork,
    IImageStorageService imageStorageService,
    IMapper mapper) : IRequestHandler<CreateStoryCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetUserByIdAsync(request.UserId.ToString());

        if (user.IsError)
        {
            return user.Errors;
        }

        var userResult = user.Value;

        var story = mapper.Map<StoryEntity>(request);

        var storyResult = await unitOfWork.Story.CreateAsync(story);

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
            story.Image = imageName;
        }

        var result = await unitOfWork.Story.SaveAsync(story);

        if (result.IsError)
        {
            return result.Errors;
        }

        return result;
    }
}