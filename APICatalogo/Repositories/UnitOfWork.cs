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

    public IProdutoRepository ProdutosRepository
    {
        get
        {
            return _produtosRepo ?? new ProdutoRepository(_context);
        }
    }

    public ICategoriaRepository CategoriasRepository
    {
        get
        {
            return _categoriasRepo ?? new CategoriaRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
