using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

/// <summary>
/// Entidade que representa um produto no catálogo
/// </summary>
/// <remarks>
/// Mapeada para a tabela "Produto" no banco de dados.
/// Relaciona-se com a entidade Categoria através da chave estrangeira CategoriaId.
/// </remarks>
[Table("Produto")]
public class Produto
{
    /// <summary>
    /// Identificador único do produto
    /// </summary>
    [Key]
    public int ProdutoId { get; set; }

    /// <summary>
    /// Nome do produto
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 80 caracteres
    /// </remarks>
    /// <example>Refrigerante</example>
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    /// <summary>
    /// Descrição detalhada do produto
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 300 caracteres
    /// </remarks>
    /// <example>Refrigerante de cola com 350ml</example>
    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }

    /// <summary>
    /// Preço do produto
    /// </summary>
    /// <remarks>
    /// Armazenado como decimal com 2 casas decimais
    /// Deve ser um valor positivo
    /// </remarks>
    /// <example>3.99</example>
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    /// <summary>
    /// URL da imagem do produto
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 300 caracteres
    /// </remarks>
    /// <example>https://exemplo.com/imgs/refri.jpg</example>
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    /// <summary>
    /// Quantidade disponível em estoque
    /// </summary>
    /// <example>300</example>
    public float Estoque { get; set; }

    /// <summary>
    /// Data de cadastro do produto no sistema
    /// </summary>
    public DateTime DataCadastro { get; set; }

    /// <summary>
    /// Identificador da categoria do produto (chave estrangeira)
    /// </summary>
    public int CategoriaId { get; set; }

    /// <summary>
    /// Objeto de categoria associado (navegação)
    /// </summary>
    /// <remarks>
    /// Ignorado na serialização JSON para evitar referências circulares
    /// </remarks>
    [JsonIgnore]
    public Categoria? Categoria { get; set; }
}
