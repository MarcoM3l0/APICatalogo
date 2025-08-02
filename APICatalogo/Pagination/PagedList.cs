namespace APICatalogo.Pagination;

/// <summary>
/// Classe genérica para implementação de listas paginadas
/// </summary>
/// <typeparam name="T">Tipo dos itens da lista</typeparam>
/// <remarks>
/// Herda de List&lt;T&gt; e adiciona metadados de paginação.
/// Útil para retornar dados paginados em APIs RESTful.
/// </remarks>
public class PagedList<T> : List<T>
{
    /// <summary>
    /// Número da página atual
    /// </summary>
    /// <example>1</example>
    public int CurrentPage { get; private set; }
    /// <summary>
    /// Número total de páginas disponíveis
    /// </summary>
    /// <example>5</example>
    public int TotalPages { get; private set; }
    /// <summary>
    /// Tamanho da página (quantidade de itens por página)
    /// </summary>
    /// <example>10</example>
    public int PageSize { get; private set; }
    /// <summary>
    /// Número total de itens na coleção original
    /// </summary>
    /// <example>50</example>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Indica se existe uma página anterior
    /// </summary>
    /// <value>True se a página atual não for a primeira, False caso contrário</value>
    public bool HasPrevious => CurrentPage > 1;
    /// <summary>
    /// Indica se existe uma próxima página
    /// </summary>
    /// <value>True se a página atual não for a última, False caso contrário</value>
    public bool HasNext => CurrentPage < TotalPages;

    /// <summary>
    /// Construtor da lista paginada
    /// </summary>
    /// <param name="item">Itens da página atual</param>
    /// <param name="count">Número total de itens na coleção</param>
    /// <param name="pageNumber">Número da página atual (baseado em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    public PagedList(List<T> item, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(item);
    }

    /// <summary>
    /// Cria uma lista paginada a partir de uma coleção IQueryable
    /// </summary>
    /// <param name="source">Fonte de dados IQueryable</param>
    /// <param name="pageNumber">Número da página desejada (baseado em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <returns>Instância de PagedList&lt;T&gt; contendo os itens da página solicitada</returns>
    /// <exception cref="ArgumentNullException">Lançada quando source é null</exception>
    /// <exception cref="ArgumentException">Lançada quando pageNumber ou pageSize são menores que 1</exception>
    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
