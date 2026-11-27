using System.Security.Claims;
using System.Text;
using ChildrenPath.Platform.Iam.Application.Internal.OutboundServices;
using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ChildrenPath.Platform.Iam.Infrastructure.Tokens.Jwt.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user)
    {
        // Lee la clave secreta desde appsettings.json ("JwtSettings:Secret")
        var secret = configuration["JwtSettings:Secret"] ?? "CLAVE_SUPER_SECRETA_POR_DEFECTO_123456";
        var key = Encoding.ASCII.GetBytes(secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(tokenDescriptor);
    }

    public async Task<int?> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var tokenHandler = new JsonWebTokenHandler();
        var secret = configuration["JwtSettings:Secret"] ?? "CLAVE_SUPER_SECRETA_POR_DEFECTO_123456";
        var key = Encoding.ASCII.GetBytes(secret);

        try
        {
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
            return userId;
        }
        catch
        {
            return null; // Si el token es inválido o expiró
        }
    }
}