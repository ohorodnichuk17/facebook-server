using ErrorOr;
using Facebook.Domain.Story;
using MediatR;

namespace Facebook.Application.UserProfile.Query.GetStoriesByUserId;

public record GetStoriesByUserIdQuery(Guid UserId) : IRequest<ErrorOr<IEnumerable<StoryEntity>>>;