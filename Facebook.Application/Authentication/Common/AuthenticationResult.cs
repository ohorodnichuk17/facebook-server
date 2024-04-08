using Facebook.Domain.User;

namespace Facebook.Application.Authentication.Common;

public record AuthenticationResult(
    Guid id,
    User User,
    string Token);