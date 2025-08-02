using AutoMapper.Configuration.Annotations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APICatalogo.Services;

/// <summary>
/// Implementação concreta do serviço de geração e validação de tokens JWT
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// Gera um token de acesso JWT
    /// </summary>
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _configuration)
    {
        var kay = _configuration.GetSection("JWT").GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Chave secreta inválida.");
        
        var privateKey = Encoding.UTF8.GetBytes(kay);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),


            Audience = _configuration.GetSection("JWT").GetValue<string>("ValidAudience"),

            Issuer = _configuration.GetSection("JWT").GetValue<string>("ValidIssuer"),
            SigningCredentials = signingCredentials
        }; 

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        return token;
    }

    /// <summary>
    /// Gera um token de refresh criptograficamente seguro
    /// </summary>
    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(secureRandomBytes);

        return Convert.ToBase64String(secureRandomBytes);
    }

    /// <summary>
    /// Valida e extrai o principal de um token expirado
    /// </summary>
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _configuration)
    {
        var secretKey = _configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("Chave inválida.");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false // Não valida o tempo de vida do token
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Token inválido.");
        }

        return principal;
    }
}
