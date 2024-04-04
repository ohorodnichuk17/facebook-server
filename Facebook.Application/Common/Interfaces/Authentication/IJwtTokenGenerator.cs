using Facebook.Domain.User;
using ErrorOr;

namespace Facebook.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    Task<ErrorOr<string>> GenerateJwtTokenAsync(User user, string role);
}