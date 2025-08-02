using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

/// <summary>
/// Objeto de Transferência de Dados (DTO) para a entidade Categoria
/// </summary>
/// <remarks>
/// Este DTO é usado para transferir dados de categoria entre as camadas da aplicação,
/// fornecendo validações básicas para as propriedades.
/// </remarks>
public class CategoriaDTO
{
    /// <summary>
    /// Identificador único da categoria
    /// </summary>
    /// <example>1</example>
    public int CategoriaId { get; set; }

    /// <summary>
    /// Nome da categoria
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 80 caracteres
    /// </remarks>
    /// <example>Lanche</example>
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    /// <summary>
    /// URL da imagem representativa da categoria
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 300 caracteres e ser uma URL válida
    /// </remarks>
    /// <example>http://exemplo.com/imagens/lanches.jpg</example>
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
}
