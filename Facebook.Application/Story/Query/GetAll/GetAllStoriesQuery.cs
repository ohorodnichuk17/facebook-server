using Facebook.Domain.Story;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Story.Query.GetAll;

public record GetAllStoriesQuery : IRequest<ErrorOr<IEnumerable<StoryEntity>>>;