using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Facebook.Application.Story.Command.Create;

public record CreateStoryCommand(
    string? Content,
    IFormFile? Image,
    Guid UserId
    ) : IRequest<ErrorOr<Unit>>;