using ErrorOr;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.UserProfile.Query.GetById;

public record GetUserProfileByIdQuery(Guid UserId) : IRequest<ErrorOr<UserProfileEntity>>;