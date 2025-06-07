namespace APICatalogo.Pagination;

public class ProdutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco;
    public string? PrecoCriterio; // Exemplo: "menor", "maior", "igual"
}
