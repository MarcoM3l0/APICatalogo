using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

/// <summary>
/// Interface específica para operações de repositório de Categorias
/// </summary>
/// <remarks>
/// Estende a interface genérica IRepository com operações específicas
/// para a entidade Categoria, incluindo métodos de paginação e filtro.
/// </remarks>
public interface ICategoriaRepository :IRepository<Categoria>
{
    /// <summary>
    /// Obtém uma lista paginada de categorias
    /// </summary>
    /// <param name="categoriasParameters">Parâmetros de paginação</param>
    /// <returns>
    /// Task contendo PagedList de Categorias ordenadas por ID
    /// </returns>
    Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);

    /// <summary>
    /// Obtém categorias filtradas por nome com paginação
    /// </summary>
    /// <param name="categoriasFiltroParams">Parâmetros de filtro e paginação</param>
    /// <returns>
    /// Task contendo PagedList de Categorias que contêm o nome especificado
    /// </returns>
    Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroParams);
}
