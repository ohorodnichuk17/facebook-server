using ErrorOr;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public record CreateStoryCommand(
    Guid Id,
    string? Content,
    byte[]? Image
    // string? Video
    ) : IRequest<ErrorOr<StoryEntity>>;