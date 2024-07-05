using Facebook.Domain.Post;
using MediatR;
using ErrorOr;

namespace Facebook.Application.Feeling.Query.GetAll;

public record GetAllFeelingsQuery() : IRequest<ErrorOr<IEnumerable<FeelingEntity>>>;