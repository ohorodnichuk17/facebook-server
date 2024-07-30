using ErrorOr;
using Facebook.Application.UserProfile.Common;
using MediatR;

namespace Facebook.Application.UserProfile.Command.DeleteAvatar;

public record DeleteAvatarCommand(
    string UserId) : IRequest<ErrorOr<DeleteAvatarResult>>;