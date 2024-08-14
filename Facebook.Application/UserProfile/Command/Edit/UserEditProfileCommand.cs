using ErrorOr;
using Facebook.Application.UserProfile.Common;
using MediatR;

namespace Facebook.Application.UserProfile.Command.Edit;
public record UserEditProfileCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    bool? isOnline,
    byte[]? CoverPhoto,
    byte[]? Avatar,
    string? Pronouns,
    string? Biography,
    bool? IsProfilePublic,
    string? Country,
    string? Region
) : IRequest<ErrorOr<EditProfileResult>>;
