using APICatalogo.context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

/// <summary>
/// Implementação concreta do repositório para a entidade Categoria
/// </summary>
/// <remarks>
/// Responsável por operações de dados específicas para categorias,
/// incluindo paginação e filtros personalizados.
/// </remarks>
public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{

    /// <summary>
    /// Inicializa uma nova instância do repositório de categorias
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todas as categorias paginadas
    /// </summary>
    /// <param name="categoriasParameters">Parâmetros de paginação</param>
    /// <returns>Lista paginada de categorias ordenadas por ID</returns>
    public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
    {
        var categorias = await GetAllAsync();

        var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

        var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);
        return resultado;
    }

    /// <summary>
    /// Obtém categorias filtradas por nome com paginação
    /// </summary>
    /// <param name="categoriasFiltroParameters">Parâmetros de filtro e paginação</param>
    /// <returns>Lista paginada de categorias que contém o nome especificado</returns>
    public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroParameters)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasFiltroParameters.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroParameters.Nome));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasFiltroParameters.PageNumber, categoriasFiltroParameters.PageSize);
        return categoriasFiltradas;
    }
}
