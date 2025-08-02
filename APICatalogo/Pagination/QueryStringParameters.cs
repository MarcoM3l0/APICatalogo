namespace APICatalogo.Pagination;

/// <summary>
/// Classe abstrata base para parâmetros de paginação via query string
/// </summary>
/// <remarks>
/// Fornece propriedades padrão para implementação de paginação em APIs,
/// incluindo controle de tamanho máximo de página.
/// </remarks>
public abstract class QueryStringParameters
{
    const int MaxPageSize = 50;

    /// <summary>
    /// Número da página atual (baseado em 1)
    /// </summary>
    /// <remarks>
    /// Valor padrão: 1 (primeira página)
    /// </remarks>
    /// <example>1</example>
    public int PageNumber { get; set; } = 1;

    private int _pageSize = MaxPageSize;
    /// <summary>
    /// Tamanho da página (quantidade de itens por página)
    /// </summary>
    /// <remarks>
    /// Valor padrão: 50 (valor máximo)
    /// Se tentar definir um valor maior que MaxPageSize, será automaticamente
    /// ajustado para o valor máximo permitido.
    /// </remarks>
    /// <example>10</example>
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
