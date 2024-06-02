using Facebook.Domain.Post;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Post.Command.Create;

public record CreatePostCommand(
    string? Title,
    string? Content,
    List<string>? Tags,
    string? Location,
    List<(byte[] Image, int Priority)> Images,
    bool IsArchive,
    Guid UserId) : IRequest<ErrorOr<Unit>>;