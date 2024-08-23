using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Story.Query.GetFriendsStories;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Story.Query.GetAll;

public class GetFriendsStoriesQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetFriendsStoriesQuery, ErrorOr<IEnumerable<StoryEntity>>>
{
    public async Task<ErrorOr<IEnumerable<StoryEntity>>> Handle(GetFriendsStoriesQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var result = await unitOfWork.Story.GetFriendsStoriesAsync(new Guid(currentUserId));

        return result;
    }
}