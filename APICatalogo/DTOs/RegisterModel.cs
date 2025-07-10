using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class RegisterModel
{
    [Required(ErrorMessage = "O usuário é obrigatorio!")]
    public string? Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "O email é obrigatorio!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatoria!")]
    public string? Password { get; set; }
}
