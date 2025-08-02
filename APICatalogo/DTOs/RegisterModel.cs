using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

/// <summary>
/// Modelo de dados para registro de novos usuários
/// </summary>
/// <remarks>
/// Contém as informações necessárias para criar uma nova conta de usuário no sistema.
/// Todas as propriedades são obrigatórias e validadas.
/// </remarks>
public class RegisterModel
{
    /// <summary>
    /// Nome de usuário para identificação no sistema
    /// </summary>
    /// <remarks>
    /// Deve ser único no sistema. Recomenda-se usar apenas letras, números e underscores.
    /// </remarks>
    /// <example>marco_melo</example>
    [Required(ErrorMessage = "O usuário é obrigatorio!")]
    public string? Username { get; set; }

    /// <summary>
    /// Endereço de e-mail do usuário
    /// </summary>
    /// <remarks>
    /// Deve ser um e-mail válido e único no sistema.
    /// </remarks>
    /// <example>marco_melo@example.com</example>
    [EmailAddress]
    [Required(ErrorMessage = "O email é obrigatorio!")]
    public string? Email { get; set; }

    /// <summary>
    /// Senha de acesso do usuário
    /// </summary>
    /// <remarks>
    /// Deve conter pelo menos 6 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.
    /// </remarks>
    /// <example>Senha@123</example>
    [Required(ErrorMessage = "A senha é obrigatoria!")]
    public string? Password { get; set; }
}
