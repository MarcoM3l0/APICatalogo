namespace APICatalogo.Repositories;

public interface IUnitOfWork
{
    IProdutoRepository ProdutosRepository { get; }
    ICategoriaRepository CategoriasRepository { get; }
    Task CommitAsync();
}
