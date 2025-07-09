using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAcesseToken(IEnumerable<Claim> claims, IConfiguration _configuration);

    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _configuration);
}
