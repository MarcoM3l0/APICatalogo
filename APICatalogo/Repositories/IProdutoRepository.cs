using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutosFiltro(ProdutosFiltroPreco produtosFiltroPreco);
    IEnumerable<Produto> GetProdutoPorCategoria(int id);
}
