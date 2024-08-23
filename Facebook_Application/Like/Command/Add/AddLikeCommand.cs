using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Like.Command.Add;

public record AddLikeCommand(
    Guid PostId
) : IRequest<ErrorOr<LikeEntity>>;
