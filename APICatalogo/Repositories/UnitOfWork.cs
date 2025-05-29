using APICatalogo.context;

namespace APICatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IProdutoRepository _produtosRepo;
    private readonly ICategoriaRepository _categoriasRepo;
    
    public readonly AppDbContext _context;


    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IProdutoRepository Produtos
    {
        get
        {
            return _produtosRepo ?? new ProdutoRepository(_context);
        }
    }

    public ICategoriaRepository Categorias
    {
        get
        {
            return _categoriasRepo ?? new CategoriaRepository(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
