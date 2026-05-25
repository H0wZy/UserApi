using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserApi.Dto;
using UserApi.Models;
using UserApi.Utils;

namespace UserApi.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public TokenDto GenerateToken(User user, string loginMethod)
    {
        var secret = config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not set.");
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];

        var expiresAt = DateTime.UtcNow.AddHours(1);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username.Value),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(ClaimTypes.SerialNumber, user.Cpf.Value),
            new(ClaimTypes.DateOfBirth, user.BirthDate.ToString("yyyy-MM-dd")),
            new(ClaimTypes.Role, user.UserType.ToDescription()),
            new(ClaimTypes.AuthenticationMethod, loginMethod),
            new(JwtRegisteredClaimNames.AuthTime,
                new DateTimeOffset(user.LastLoginAt!.Value).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: expiresAt,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenDto(AccessToken: accessToken, ExpiresAt: expiresAt, LoginMethod: loginMethod);
    }
}