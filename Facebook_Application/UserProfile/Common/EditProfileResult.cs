using Facebook.Domain.User;

namespace Facebook.Application.UserProfile.Common;

public record EditProfileResult(
    UserProfileEntity UserProfile,
    string Token);