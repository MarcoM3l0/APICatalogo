namespace APICatalogo.Pagination;

/// <summary>
/// Parâmetros para filtragem de produtos por preço com suporte a paginação
/// </summary>
/// <remarks>
/// Herda de QueryStringParameters para incluir parâmetros de paginação
/// e adiciona propriedades para filtragem por preço.
/// </remarks>
public class ProdutosFiltroPreco : QueryStringParameters
{
    /// <summary>
    /// Valor de preço para filtro
    /// </summary>
    /// <remarks>
    /// Quando combinado com PrecoCriterio, filtra produtos com base no valor especificado.
    /// Deve ser um valor decimal positivo.
    /// </remarks>
    /// <example>10.50</example>
    public decimal? Preco { get; set; }
    /// <summary>
    /// Critério de comparação para o filtro de preço
    /// </summary>
    /// <remarks>
    /// Valores aceitos:
    /// - "menor": produtos com preço menor que o valor especificado
    /// - "maior": produtos com preço maior que o valor especificado
    /// - "igual": produtos com preço igual ao valor especificado
    /// </remarks>
    /// <example>menor</example>
    public string? PrecoCriterio { get; set; } // Exemplo: "menor", "maior", "igual"
}
