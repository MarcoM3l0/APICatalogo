﻿namespace APICatalogo.Pagination;

public class ProdutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public string? PrecoCriterio { get; set; } // Exemplo: "menor", "maior", "igual"
}
