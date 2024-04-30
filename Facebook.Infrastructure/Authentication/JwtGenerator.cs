using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ErrorOr;
using Facebook.Domain.User;

namespace Facebook.Infrastructure.Authentication;

public class JwtGenerator : IJwtGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(IDateTimeProvider dateTimeProvider, 
        IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }
    
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
            new Claim("EmailConfirm", userEntity.EmailConfirmed.ToString()),
            new Claim(ClaimTypes.MobilePhone, userEntity.PhoneNumber ?? ""),
            new Claim(ClaimTypes.Role, role),
            new Claim("Avatar", userEntity.Avatar ?? ""),
        };
        
        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}