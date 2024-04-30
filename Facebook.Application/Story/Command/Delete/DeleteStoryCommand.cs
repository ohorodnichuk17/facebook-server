using MediatR;
using ErrorOr;

namespace Facebook.Application.Story.Command.Delete;

public record DeleteStoryCommand (
    Guid Id
    ) : IRequest<ErrorOr<bool>>;