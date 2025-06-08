using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository :IRepository<Categoria>
{
    Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
    Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroParams);
}
