using APICatalogo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.context;

/// <summary>
/// Representa o contexto do banco dados da aplicação, usado para interagir com o banco de dados.
/// </summary>
/// <remarks>
/// Esta classe herda de IdentityDbContext para gerenciar autenticação e autorização,
/// e inclui DbSets para as entidades Categoria e Produto.
/// </remarks>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Inicializa uma nova instância do contexto do banco de dados.
    /// </summary>
    /// <param name="options">As opções de configuração para este contexto.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Obtém ou define o conjunto de entidades Categoria no banco de dados.
    /// </summary>
    public DbSet<Categoria> Categorias { get; set; }
    /// <summary>
    /// Obtém ou define o conjunto de entidades Produto no banco de dados.
    /// </summary>
    public DbSet<Produto> Produtos { get; set; }

    /// <summary>
    /// Configura o modelo de dados durante a criação do contexto.
    /// </summary>
    /// <param name="modelBuilder">O construtor usado para criar o modelo para este contexto.</param>
    /// <remarks>
    /// Este método é chamado quando o modelo para um contexto derivado é inicializado.
    /// Pode ser usado para configurar relações, chaves, restrições e outras configurações do modelo.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

