using MediatR;
using ErrorOr;

namespace Facebook.Application.Feeling.Command.Add;

public record AddFeelingCommand(
    string Name,
    string Emoji) : IRequest<ErrorOr<Guid>>;