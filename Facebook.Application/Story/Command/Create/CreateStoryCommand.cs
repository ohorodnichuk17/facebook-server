using ErrorOr;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public record CreateStoryCommand(
    string? Content,
    byte[]? Image,
    Guid UserId
    // string? Video
    ) : IRequest<ErrorOr<Unit>>;