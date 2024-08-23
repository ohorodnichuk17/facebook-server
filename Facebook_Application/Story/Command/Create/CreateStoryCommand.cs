using ErrorOr;
using MediatR;

namespace Facebook.Application.Story.Command.Create;

public record CreateStoryCommand(
    string? Content,
    byte[] Image
    ) : IRequest<ErrorOr<Unit>>;