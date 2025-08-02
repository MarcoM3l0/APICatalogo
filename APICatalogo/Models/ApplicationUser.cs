using Microsoft.AspNetCore.Identity;

namespace APICatalogo.Models;

/// <summary>
/// Classe que estende IdentityUser para adicionar funcionalidades de autenticação JWT
/// </summary>
/// <remarks>
/// Esta classe herda de IdentityUser e adiciona propriedades para gerenciamento
/// de tokens de refresh para autenticação JWT.
/// </remarks>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Token de refresh para renovação do token JWT
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Data e hora de expiração do token de refresh
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; set; } 
}
