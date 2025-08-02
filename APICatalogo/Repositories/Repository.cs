using APICatalogo.context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

/// <summary>
/// Implementação genérica base para operações de repositório
/// </summary>
/// <typeparam name="T">Tipo da entidade gerenciada pelo repositório</typeparam>
/// <remarks>
/// Fornece implementação concreta das operações CRUD básicas
/// utilizando Entity Framework Core como ORM.
/// </remarks>
public class Repository<T> : IRepository<T> where T : class
{
    /// <summary>
    /// Contexto do banco de dados utilizado pelo repositório
    /// </summary>
    protected readonly AppDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância do repositório genérico
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public Repository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém todos os registros da entidade de forma assíncrona
    /// </summary>
    /// <returns>
    /// Task contendo IEnumerable com todos os registros da entidade
    /// </returns>
    /// <remarks>
    /// Utiliza AsNoTracking() para melhor performance em operações somente leitura
    /// </remarks>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Obtém um registro específico baseado em um predicado
    /// </summary>
    /// <param name="predicate">Expressão lambda para filtrar o registro</param>
    /// <returns>
    /// Task contendo o registro encontrado ou null se não existir
    /// </returns>
    public async Task<T?> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Cria um novo registro da entidade
    /// </summary>
    /// <param name="entity">Entidade a ser criada</param>
    /// <returns>
    /// A entidade criada com possíveis atualizações (como ID gerado)
    /// </returns>
    /// <remarks>
    /// Observação: Requer chamada explícita a SaveChanges/SaveChangesAsync
    /// para persistir as alterações no banco de dados
    /// </remarks>
    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        //_context.SaveChanges(); // Intencionalmente comentado para controle explícito
        return entity;
    }

    /// <summary>
    /// Atualiza um registro existente da entidade
    /// </summary>
    /// <param name="entity">Entidade com as alterações a serem persistidas</param>
    /// <returns>
    /// A entidade atualizada
    /// </returns>
    /// <remarks>
    /// Observação: Requer chamada explícita a SaveChanges/SaveChangesAsync
    /// para persistir as alterações no banco de dados
    /// </remarks>
    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        //_context.SaveChanges(); // Intencionalmente comentado para controle explícito
        return entity;
    }

    /// <summary>
    /// Remove um registro da entidade
    /// </summary>
    /// <param name="entity">Entidade a ser removida</param>
    /// <returns>
    /// A entidade que foi removida
    /// </returns>
    /// <remarks>
    /// Observação: Requer chamada explícita a SaveChanges/SaveChangesAsync
    /// para persistir as alterações no banco de dados
    /// </remarks>
    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        //_context.SaveChanges(); // Intencionalmente comentado para controle explícito
        return entity;
    }

}
