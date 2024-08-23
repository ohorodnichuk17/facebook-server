using ErrorOr;
using MediatR;

namespace Facebook.Application.Feeling.Command.Delete;

public record DeleteFeelingCommand(
    Guid Id) : IRequest<ErrorOr<bool>>;