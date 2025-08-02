using APICatalogo.context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

/// <summary>
/// Implementação concreta do repositório para a entidade Produto
/// </summary>
/// <remarks>
/// Responsável por operações de dados específicas para produtos,
/// incluindo paginação, filtros por preço e busca por categoria.
/// </remarks>
public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    /// <summary>
    /// Inicializa uma nova instância do repositório de produtos
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todos os produtos de uma categoria específica
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>
    /// Task contendo IEnumerable de Produtos pertencentes à categoria especificada
    /// </returns>
    public async Task<IEnumerable<Produto>> GetProdutoPorCategoriaAsync(int id)
    {
        var produto = await GetAllAsync();
        return produto.Where(c => c.CategoriaId == id);
    }

    /// <summary>
    /// Obtém uma lista paginada de produtos ordenados por ID
    /// </summary>
    /// <param name="produtosParameters">Parâmetros de paginação</param>
    /// <returns>
    /// Task contendo PagedList de Produtos com metadados de paginação
    /// </returns>
    public async Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

        var resultado = PagedList<Produto>.ToPagedList(produtosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);
        return resultado;
    }

    /// <summary>
    /// Obtém produtos filtrados por preço com paginação
    /// </summary>
    /// <param name="produtosFiltroParams">Parâmetros de filtro por preço e paginação</param>
    /// <returns>
    /// Task contendo PagedList de Produtos que atendem aos critérios de preço
    /// </returns>
    public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
    {
        var produtos = await GetAllAsync();

        if (produtosFiltroParams.Preco.HasValue && !String.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
        {
            if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
            else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
            }
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

        return produtosFiltrados; 

    }

    //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //        .Skip(produtosParameters.PageSize * (produtosParameters.PageNumber - 1))
    //        .Take(produtosParameters.PageSize).ToList();
    //}
}
