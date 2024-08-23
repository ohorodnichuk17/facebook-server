using ErrorOr;
using MediatR;

namespace Facebook.Application.Story.Command.Delete;

public record DeleteStoryCommand(Guid Id) : IRequest<ErrorOr<bool>>
{
    public DeleteStoryCommand() : this(Guid.Empty)
    {
    }
    
    public DeleteStoryCommand(string requestId) : this(Guid.Parse(requestId))
    {
    }
}