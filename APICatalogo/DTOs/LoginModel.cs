using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

/// <summary>
/// Modelo de dados para autenticação de usuário
/// </summary>
/// <remarks>
/// Contém as credenciais necessárias para o processo de login.
/// Todas as propriedades são obrigatórias.
/// </remarks>
public class LoginModel
{
    /// <summary>
    /// Nome de usuário para autenticação
    /// </summary>
    /// <remarks>
    /// Deve corresponder a um nome de usuário válido cadastrado no sistema.
    /// </remarks>
    /// <example>admin</example>
    [Required(ErrorMessage = "O usuário é obrigatorio!")]
    public string? Username { get; set; }

    /// <summary>
    /// Senha do usuário
    /// </summary>
    /// <remarks>
    /// A senha deve corresponder à senha registrada para o usuário.
    /// Recomenda-se usar senhas fortes com combinação de caracteres.
    /// </remarks>
    /// <example>Senha@123</example>
    [Required(ErrorMessage = "A senha é obrigatoria!")]
    public string? Password { get; set; }
}
