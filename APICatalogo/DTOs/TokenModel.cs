namespace APICatalogo.DTOs;

/// <summary>
/// Modelo de dados para tokens de autenticação
/// </summary>
/// <remarks>
/// Contém os tokens de acesso e refresh para autenticação JWT.
/// Utilizado nas operações de login e renovação de token.
/// </remarks>
public class TokenModel
{
    /// <summary>
    /// Token de acesso JWT
    /// </summary>
    /// <remarks>
    /// Token de curta duração usado para autenticar requisições.
    /// Deve ser enviado no header Authorization como "Bearer token".
    /// </remarks>
    /// <example>eyJhb.eyJzdW.SflKxwR</example>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Token de refresh
    /// </summary>
    /// <remarks>
    /// Token de longa duração usado para obter novos tokens de acesso.
    /// Deve ser armazenado de forma segura no cliente.
    /// </remarks>
    /// <example>5e-2b-4f-7c-f6a5b4c</example>
    public string? RefreshToken { get; set; }
}
