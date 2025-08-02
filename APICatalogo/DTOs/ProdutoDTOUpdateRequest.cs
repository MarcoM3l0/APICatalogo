
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

/// <summary>
/// DTO para atualização de produtos (request) com validação personalizada
/// </summary>
/// <remarks>
/// Inclui validações para estoque e data de cadastro,
/// implementando IValidatableObject para validação customizada.
/// </remarks>
public class ProdutoDTOUpdateRequest : IValidatableObject
{
    /// <summary>
    /// Quantidade em estoque do produto
    /// </summary>
    /// <remarks>
    /// Deve ser um valor entre 1 e 9999
    /// </remarks>
    /// <example>100</example>
    [Range(1, 9999, ErrorMessage = "O estoque deve ter entre 1 e 9999")]
    public float Estoque { get; set; }

    /// <summary>
    /// Data de cadastro do produto
    /// </summary>
    /// <remarks>
    /// Deve ser uma data futura (maior que a data atual)
    /// </remarks>
    public DateTime DataCadastro { get; set; }

    /// <summary>
    /// Realiza validação customizada do objeto
    /// </summary>
    /// <param name="validationContext">Contexto de validação</param>
    /// <returns>
    /// Lista de resultados de validação. Retorna erro se a data de cadastro
    /// não for futura.
    /// </returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(DataCadastro.Date <= DateTime.Now.Date)
        {
            yield return new ValidationResult("A data de cadastro deve ser maior que a data atual.", new[] { nameof(this.DataCadastro) });
        }
    }
}
