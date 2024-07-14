using ErrorOr;
using MediatR;

namespace Facebook.Application.UserProfile.Command.DeleteAvatar;

public record DeleteAvatarCommand(
    string UserId) : IRequest<ErrorOr<bool>>;