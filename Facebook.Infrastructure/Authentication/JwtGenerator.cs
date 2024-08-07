using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Facebook.Infrastructure.Authentication;

public class JwtGenerator(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtOptions)
    : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<string> GenerateJwtTokenAsync(UserEntity userEntity, string role)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userEntity.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, userEntity.FirstName ?? ""),
            new Claim(JwtRegisteredClaimNames.FamilyName, userEntity.LastName ?? ""),
            new Claim(JwtRegisteredClaimNames.Email, userEntity.Email!),
            new Claim(JwtRegisteredClaimNames.Birthdate, userEntity.Birthday.ToString("yyyy-MM-dd")),
            new Claim("EmailConfirm", userEntity.EmailConfirmed.ToString()),
            new Claim(ClaimTypes.MobilePhone, userEntity.PhoneNumber ?? ""),
            new Claim(ClaimTypes.Role, role),
            new Claim("Avatar", userEntity.Avatar ?? ""),
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}