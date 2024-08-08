using ErrorOr;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Story.Query.GetFriendsStories;

public record GetFriendsStoriesQuery : IRequest<ErrorOr<IEnumerable<StoryEntity>>>;