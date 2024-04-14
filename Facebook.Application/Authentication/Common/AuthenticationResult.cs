using Facebook.Domain.UserEntity;

namespace Facebook.Application.Authentication.Common;

public record AuthenticationResult(
    Guid id,
    UserEntity User,
    string Token);