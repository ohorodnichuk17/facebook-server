using ErrorOr;
using MediatR;

namespace Facebook.Application.Post.Command.Create;
public class ImageWithPriority
{
    public byte[] Image { get; set; }
    public int Priority { get; set; }
}
public record CreatePostCommand(
    string? Title,
    string? Content,
    List<string>? Tags,
    string? Location,
    List<ImageWithPriority> Images,
    bool IsArchive,
    Guid UserId,
    Guid? FeelingId,
    Guid? ActionId,
    Guid? SubActionId
    ) : IRequest<ErrorOr<Unit>>;