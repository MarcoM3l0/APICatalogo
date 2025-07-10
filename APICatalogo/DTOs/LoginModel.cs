using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class LoginModel
{
    [Required(ErrorMessage = "O usuário é obrigatorio!")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "A senha é obrigatoria!")]
    public string? Password { get; set; }
}
