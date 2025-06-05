using APICatalogo.context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Produto> GetProdutoPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }

    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        return GetAll()
            .OrderBy(p => p.Nome)
            .Skip(produtosParameters.PageSize * (produtosParameters.PageNumber - 1))
            .Take(produtosParameters.PageSize).ToList();
    }
}
