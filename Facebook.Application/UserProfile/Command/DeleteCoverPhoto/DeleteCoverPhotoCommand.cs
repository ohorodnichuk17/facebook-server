using ErrorOr;
using MediatR;

namespace Facebook.Application.UserProfile.Command.DeleteCoverPhoto;

public record DeleteCoverPhotoCommand(
    string UserId) : IRequest<ErrorOr<bool>>;