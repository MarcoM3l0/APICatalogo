using APICatalogo.context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoriaRepository> _logger;

    public CategoriaRepository(AppDbContext context, ILogger<CategoriaRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<Categoria> GetCategorias()
    {
        return _context.Categorias.ToList();
    }
    public Categoria? GetCategoria(int id)
    {
        return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
    }

    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
        return _context.Categorias.Include(c => c.Produtos).ToList();
    }

    public Categoria Create(Categoria categoria)
    {
        if (categoria == null)
        {
            _logger.LogError("Post - Categoria não pode ser nula ao criar.");
            throw new InvalidOperationException("Categoria não pode ser nula");
        }

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return categoria;
    }

    public Categoria Update(Categoria categoria)
    {
        if (categoria == null) 
        { 
            _logger.LogError("Put - Categoria não pode ser nula ao atualizar.");
            throw new ArgumentNullException(nameof(categoria), "Categoria não pode ser nula");
        }

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return categoria;
    }

    public Categoria Delete(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria == null) 
        { 
            _logger.LogWarning($"Delete - Categoria com id={id} não encontrada.");
            throw new KeyNotFoundException($"Categoria com id={id} não encontrada.");
        }

        _context.Categorias.Remove(categoria);
       _context.SaveChanges();
       return categoria;
    }
}
