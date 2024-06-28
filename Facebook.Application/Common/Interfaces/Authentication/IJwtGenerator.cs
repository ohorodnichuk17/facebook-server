using ErrorOr;
using Facebook.Domain.User;

namespace Facebook.Application.Common.Interfaces.Authentication;

public interface IJwtGenerator
{
    Task<string> GenerateJwtTokenAsync(UserEntity user, string role);
}