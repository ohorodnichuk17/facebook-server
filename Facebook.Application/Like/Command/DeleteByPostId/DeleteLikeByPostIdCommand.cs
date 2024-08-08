using ErrorOr;
using MediatR;
public record DeleteLikeByPostIdCommand(
    Guid PostId
) : IRequest<ErrorOr<bool>>;