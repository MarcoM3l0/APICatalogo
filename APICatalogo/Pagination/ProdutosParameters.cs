namespace APICatalogo.Pagination;

/// <summary>
/// Parâmetros de paginação para listagem de produtos
/// </summary>
/// <remarks>
/// Herda os parâmetros básicos de paginação de QueryStringParameters.
/// Pode ser utilizado diretamente para paginação simples ou estendido
/// para adicionar filtros específicos para produtos.
/// </remarks>
public class ProdutosParameters : QueryStringParameters
{
    // Esta classe está vazia intencionalmente, mantendo apenas
    // os parâmetros de paginação herdados de QueryStringParameters.
    // Pode ser estendida no futuro para adicionar parâmetros específicos
    // de filtro para categorias.
}
