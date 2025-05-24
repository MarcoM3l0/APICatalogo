using APICatalogo.context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoriasController> _logger;
    public CategoriasController(AppDbContext contexto, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _context = contexto;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {

        try
        {
            _logger.LogInformation("=========== Get - categoria ===========");

            var categorias = await _context.Categorias.ToListAsync();

            if (categorias is null)
            {
                return NotFound("Categorias não encontradas...");
            }
            return categorias;

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao se comunicar com o servidor...");
        }
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        //throw new Exception("Erro ao buscar categoria..."); // Simulando erro para teste do middleware
        //throw new ArgumentException("Ocorreu um erro no tratamento de request"); // Simulando erro para teste
            
        
        var categoria = await _context.Categorias.FindAsync(id);

        _logger.LogInformation($"=========== Get - categoria/id={id} ===========");

        if (categoria is null)
        {
            _logger.LogInformation("=========== Categoria não encontrada ===========");
            return NotFound($"Categoria com id={id} não encontrada...");
        }
        return categoria;
        
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
    {
        _logger.LogInformation("=========== Get - categoria/produto ===========");

        var categorias = await _context.Categorias.Include(x => x.Produtos).ToListAsync();
        if (categorias is null)
        {
            return NotFound("Categorias não encontradas...");
        }
        return categorias;
        
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest("Categoria é nula...");

        _context.Categorias.Add(categoria);
        _context.SaveChanges();
        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        
        if (id != categoria.CategoriaId)
        {
            return BadRequest($"Categoria com id={id} não encontrada...");
        }
        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(categoria);
        
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
    {
        var categoria = _context.Categorias.Find(id);
        if (categoria is null)
        {
            return NotFound($"Categoria com id={id} não encontrada...");
        }
        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return categoria;
    }
}
