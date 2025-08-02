using APICatalogo.Models;

namespace APICatalogo.DTOs;

/// <summary>
/// DTO para resposta de atualização de produto
/// </summary>
/// <remarks>
/// Contém todos os dados do produto após uma operação de atualização,
/// incluindo informações da categoria associada.
/// </remarks>
public class ProdutoDTOUpdateResponse
{
    /// <summary>
    /// Identificador único do produto
    /// </summary>
    /// <example>1</example>
    public int ProdutoId { get; set; }

    /// <summary>
    /// Nome do produto
    /// </summary>
    /// <example>Cachorro Quente</example>
    public string? Nome { get; set; }

    /// <summary>
    /// Descrição detalhada do produto
    /// </summary>
    /// <example>Um delicioso cachorro quente com salsicha, queijo e molho especial</example>
    public string? Descricao { get; set; }

    /// <summary>
    /// Preço atual do produto
    /// </summary>
    /// <example>3499.99</example>
    public decimal Preco { get; set; }

    /// <summary>
    /// URL da imagem do produto
    /// </summary>
    /// <example>https://exemplo.com/imgs/cachorro-quente.jpg</example>
    public string? ImagemUrl { get; set; }

    /// <summary>
    /// Quantidade disponível em estoque
    /// </summary>
    /// <example>20</example>
    public float Estoque { get; set; }

    /// <summary>
    /// Data de cadastro do produto no sistema
    /// </summary>
    public DateTime DataCadastro { get; set; }

    /// <summary>
    /// ID da categoria a que o produto pertence
    /// </summary>
    /// <example>2</example>
    public int CategoriaId { get; set; }

    /// <summary>
    /// Objeto contendo os detalhes da categoria do produto
    /// </summary>
    public Categoria? Categoria { get; set; }
}