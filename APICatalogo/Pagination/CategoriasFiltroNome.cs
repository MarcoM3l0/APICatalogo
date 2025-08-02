namespace APICatalogo.Pagination;

/// <summary>
/// Classe para filtragem de categorias por nome com suporte a paginação
/// </summary>
/// <remarks>
/// Herda de QueryStringParameters para incluir parâmetros de paginação
/// e adiciona a propriedade Nome para filtragem específica.
/// </remarks>
public class CategoriasFiltroNome : QueryStringParameters
{
    /// <summary>
    /// Nome ou parte do nome para filtro das categorias
    /// </summary>
    /// <remarks>
    /// Quando informado, retorna apenas categorias cujos nomes contenham este valor.
    /// </remarks>
    /// <example>lanches</example>
    public string? Nome { get; set; }
}
