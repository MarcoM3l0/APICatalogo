using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models;

/// <summary>
/// Entidade que representa uma categoria de produtos no catálogo
/// </summary>
/// <remarks>
/// Mapeada para a tabela "Categoria" no banco de dados.
/// Contém uma coleção de produtos associados a esta categoria.
/// </remarks>
[Table("Categoria")]
public class Categoria
{
    /// <summary>
    /// Inicializa uma nova instância da classe Categoria
    /// </summary>
    /// <remarks>
    /// Inicializa a coleção de produtos para evitar referências nulas
    /// </remarks>
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    /// <summary>
    /// Identificador único da categoria
    /// </summary>
    [Key]
    public int CategoriaId { get; set; }

    /// <summary>
    /// Nome da categoria
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 80 caracteres
    /// </remarks>
    /// <example>Bebida</example>
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    /// <summary>
    /// URL da imagem representativa da categoria
    /// </summary>
    /// <remarks>
    /// Deve ter entre 1 e 300 caracteres
    /// </remarks>
    /// <example>http://exemplo.com/imagens/bebidas.jpg</example>
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    /// <summary>
    /// Coleção de produtos associados a esta categoria
    /// </summary>
    /// <remarks>
    /// Relacionamento um-para-muitos com a entidade Produto
    /// </remarks>
    public ICollection<Produto>? Produtos { get; set; }  
}
