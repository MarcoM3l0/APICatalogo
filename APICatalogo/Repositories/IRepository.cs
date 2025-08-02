using System.Linq.Expressions;

namespace APICatalogo.Repositories;

/// <summary>
/// Interface genérica para operações básicas de repositório
/// </summary>
/// <typeparam name="T">Tipo da entidade gerenciada pelo repositório</typeparam>
/// <remarks>
/// Define as operações CRUD básicas (Create, Read, Update, Delete)
/// de forma genérica para ser implementada por repositórios específicos.
/// </remarks>
public interface IRepository <T>
{
    /// <summary>
    /// Obtém todos os registros da entidade de forma assíncrona
    /// </summary>
    /// <returns>
    /// Task contendo IEnumerable com todos os registros da entidade
    /// </returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtém um registro específico baseado em um predicado
    /// </summary>
    /// <param name="predicate">Expressão lambda para filtrar o registro</param>
    /// <returns>
    /// Task contendo o registro encontrado ou null se não existir
    /// </returns>
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Cria um novo registro da entidade
    /// </summary>
    /// <param name="entity">Entidade a ser criada</param>
    /// <returns>
    /// A entidade criada com possíveis atualizações (como ID gerado)
    /// </returns>
    T Create(T entity);

    /// <summary>
    /// Atualiza um registro existente da entidade
    /// </summary>
    /// <param name="entity">Entidade com as alterações a serem persistidas</param>
    /// <returns>
    /// A entidade atualizada
    /// </returns>
    T Update(T entity);

    /// <summary>
    /// Remove um registro da entidade
    /// </summary>
    /// <param name="entity">Entidade a ser removida</param>
    /// <returns>
    /// A entidade que foi removida
    /// </returns>
    T Delete(T entity);
}
