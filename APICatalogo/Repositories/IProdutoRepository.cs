using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

/// <summary>
/// Interface específica para operações de repositório de Produtos
/// </summary>
/// <remarks>
/// Estende a interface genérica IRepository com operações específicas
/// para a entidade Produto, incluindo métodos de paginação, filtro por preço
/// e busca por categoria.
/// </remarks>
public interface IProdutoRepository : IRepository<Produto>
{
    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);

    /// <summary>
    /// Obtém uma lista paginada de produtos
    /// </summary>
    /// <param name="produtosParameters">Parâmetros de paginação</param>
    /// <returns>
    /// Task contendo PagedList de Produtos com metadados de paginação
    /// </returns>
    Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);

    /// <summary>
    /// Obtém produtos filtrados por preço com paginação
    /// </summary>
    /// <param name="produtosFiltroPreco">Parâmetros de filtro por preço e paginação</param>
    /// <returns>
    /// Task contendo PagedList de Produtos que atendem aos critérios de preço
    /// </returns>
    Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);

    /// <summary>
    /// Obtém todos os produtos de uma categoria específica
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>
    /// Task contendo IEnumerable de Produtos pertencentes à categoria especificada
    /// </returns>
    Task<IEnumerable<Produto>> GetProdutoPorCategoriaAsync(int id);
}
