using Facebook.Domain.Story;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Story.Query.GetById;

public record GetStoryByIdQuery(Guid Id) : IRequest<ErrorOr<StoryEntity>>;