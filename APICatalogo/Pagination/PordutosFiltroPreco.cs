namespace APICatalogo.Pagination;

public class PordutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco;
    public string? PrecoCriterio; // Exemplo: "menor", "maior", "igual"
}
