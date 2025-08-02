using APICatalogo.context;

namespace APICatalogo.Repositories;

/// <summary>
/// Implementação concreta do padrão Unit of Work
/// </summary>
/// <remarks>
/// Coordena o trabalho de múltiplos repositórios, garantindo que operações
/// sejam executadas atomicamente como uma única transação.
/// Gerencia o ciclo de vida do contexto do banco de dados.
/// </remarks>
public class UnitOfWork : IUnitOfWork
{
    private readonly IProdutoRepository _produtosRepo;
    private readonly ICategoriaRepository _categoriasRepo;

    /// <summary>
    /// Contexto do banco de dados utilizado pelo UnitOfWork
    /// </summary>
    public readonly AppDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância do UnitOfWork
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public UnitOfWork(AppDbContext context) => _context = context;

    /// <summary>
    /// Acesso ao repositório de produtos
    /// </summary>
    /// <remarks>
    /// Implementa o padrão lazy initialization - a instância só é criada quando acessada
    /// </remarks>
    public IProdutoRepository ProdutosRepository
    {
        get
        {
            return _produtosRepo ?? new ProdutoRepository(_context);
        }
    }

    /// <summary>
    /// Acesso ao repositório de categorias
    /// </summary>
    /// <remarks>
    /// Implementa o padrão lazy initialization - a instância só é criada quando acessada
    /// </remarks>
    public ICategoriaRepository CategoriasRepository
    {
        get
        {
            return _categoriasRepo ?? new CategoriaRepository(_context);
        }
    }

    /// <summary>
    /// Persiste todas as alterações no banco de dados de forma assíncrona
    /// </summary>
    /// <returns>Task que representa a operação assíncrona</returns>
    /// <exception cref="ObjectDisposedException">
    /// Lançada se o UnitOfWork já foi descartado
    /// </exception>
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Libera os recursos do contexto do banco de dados
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}
