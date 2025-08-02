namespace APICatalogo.Repositories;

/// <summary>
/// Interface para o padrão Unit of Work
/// </summary>
/// <remarks>
/// Coordena o trabalho de múltiplos repositórios, garantindo que operações
/// sejam executadas atomicamente como uma única transação.
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Repositório para operações com produtos
    /// </summary>
    IProdutoRepository ProdutosRepository { get; }

    /// <summary>
    /// Repositório para operações com categorias
    /// </summary>
    ICategoriaRepository CategoriasRepository { get; }

    /// <summary>
    /// Persiste todas as alterações no banco de dados de forma assíncrona
    /// </summary>
    /// <returns>
    /// Task que representa a operação assíncrona
    /// </returns>
    Task CommitAsync();
}
