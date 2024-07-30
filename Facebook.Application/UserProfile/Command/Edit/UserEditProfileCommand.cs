using ErrorOr;
using Facebook.Application.UserProfile.Common;
using MediatR;

namespace Facebook.Application.UserProfile.Command.Edit;
public record UserEditProfileCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    byte[]? CoverPhoto,
    byte[]? Avatar,
    string? Pronouns,
    string? Biography,
    bool? IsProfilePublic,
    bool? isBlocked,
    string? Country,
    string? Region
) : IRequest<ErrorOr<EditProfileResult>>;
