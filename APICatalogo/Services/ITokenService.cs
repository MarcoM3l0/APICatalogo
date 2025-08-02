using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Services;

/// <summary>
/// Interface para serviços de geração e validação de tokens JWT
/// </summary>
/// <remarks>
/// Define as operações necessárias para gerenciamento de tokens de autenticação,
/// incluindo geração de tokens de acesso e refresh, e validação de tokens expirados.
/// </remarks>
public interface ITokenService
{
    /// <summary>
    /// Gera um token de acesso JWT
    /// </summary>
    /// <param name="claims">Lista de claims (reivindicações) para incluir no token</param>
    /// <param name="_configuration">Configuração da aplicação para obter chaves e configurações JWT</param>
    /// <returns>JwtSecurityToken contendo o token de acesso gerado</returns>
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _configuration);

    /// <summary>
    /// Gera um token de refresh
    /// </summary>
    /// <returns>String contendo o token de refresh gerado</returns>
    /// <remarks>
    /// O token de refresh é tipicamente uma string aleatória criptograficamente segura
    /// </remarks>
    string GenerateRefreshToken();

    /// <summary>
    /// Obtém o principal (identidade) de um token JWT expirado
    /// </summary>
    /// <param name="token">Token JWT expirado</param>
    /// <param name="_configuration">Configuração da aplicação para obter chaves e configurações JWT</param>
    /// <returns>ClaimsPrincipal extraído do token</returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _configuration);
}
