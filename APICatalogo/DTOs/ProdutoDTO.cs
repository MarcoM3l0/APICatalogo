using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

/// <summary>
/// Objeto de Transferência de Dados (DTO) para a entidade Produto
/// </summary>
/// <remarks>
/// Este DTO representa os dados de um produto para operações de leitura e escrita,
/// incluindo validações básicas para as propriedades.
/// </remarks>
public class ProdutoDTO
{
    /// <summary>
    /// Identificador único do produto
    /// </summary>
    /// <example>1</example>
    public int ProdutoId { get; set; }

    /// <summary>
    /// Nome do produto
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 80 caracteres
    /// </remarks>
    /// <example>Hamburguer</example>
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    /// <summary>
    /// Descrição detalhada do produto
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 300 caracteres
    /// </remarks>
    /// <example>Hamburguer com carne bovina, queijo e molho especial</example>
    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }

    /// <summary>
    /// Preço do produto
    /// </summary>
    /// <remarks>
    /// Deve ser um valor positivo
    /// </remarks>
    /// <example>19.90</example>
    [Required]
    public decimal Preco { get; set; }

    /// <summary>
    /// URL da imagem do produto
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 300 caracteres
    /// </remarks>
    /// <example>http://exemplo.com/imagens/hamburguer.jpg</example>
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    /// <summary>
    /// Identificador da categoria associada ao produto
    /// </summary>
    /// <example>1</example>
    public int CategoriaId { get; set; }
}
