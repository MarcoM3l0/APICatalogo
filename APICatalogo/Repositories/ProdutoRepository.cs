using APICatalogo.context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProdutoRepository> _logger;

    public ProdutoRepository(AppDbContext context, ILogger<ProdutoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Produto> GetProdutos()
    {
        return _context.Produtos;
    }
    public Produto? GetProduto(int id)
    {
        return _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    }

    public Produto Create(Produto produto)
    {
        if(produto is null)
        {
            _logger.LogError("Post - Produto não pode ser nulo ao criar.");
            throw new InvalidOperationException("Produto não pode ser nulo");
        }

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return produto;
    }

    public bool Update(Produto produto)
    {
        if(produto is null)
        {
            _logger.LogError("Put - Produto não pode ser nulo ao atualizar.");
            throw new ArgumentNullException(nameof(produto), "Produto não pode ser nulo");
        }

        if(_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId)){

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return true;

        }

        return false;
    }
    public bool Delete(int id)
    {
        var produto = _context.Produtos.Find(id);

        if(produto is not null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}
