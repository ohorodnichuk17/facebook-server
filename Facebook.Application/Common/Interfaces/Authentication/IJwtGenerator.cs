using Facebook.Domain.UserEntity;
using ErrorOr;

namespace Facebook.Application.Common.Interfaces.Authentication;

public interface IJwtGenerator
{
    Task<string> GenerateJwtTokenAsync(UserEntity user, string role);
}