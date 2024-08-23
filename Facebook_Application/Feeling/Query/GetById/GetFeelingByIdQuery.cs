using Facebook.Domain.Post;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Feeling.Query.GetById;

public record GetFeelingByIdQuery(Guid Id) : IRequest<ErrorOr<FeelingEntity>>;