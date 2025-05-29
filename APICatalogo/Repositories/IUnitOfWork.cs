namespace APICatalogo.Repositories;

public interface IUnitOfWork
{
    IProdutoRepository Produtos { get; }
    ICategoriaRepository Categorias { get; }
    void commit();
}
