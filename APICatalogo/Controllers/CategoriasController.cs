using APICatalogo.context;
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
    public CategoriasController(AppDbContext contexto, IConfiguration configuration)
    {
        _context = contexto;
        _configuration = configuration;
    }


    [HttpGet("LerArquivoConfigucao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];
        var secao1 = _configuration["secao1:chave2"];

        return $"Valor1: {valor1} - Valor2: {valor2} - Secao1: {secao1}";
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {

        try
        {
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
        try
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria is null)
            {
                return NotFound($"Categoria com id={id} não encontrada...");
            }
            return categoria;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao se comunicar com o servidor...");
        }
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
    {
        try
        {
            var categorias = await _context.Categorias.Include(x => x.Produtos).ToListAsync();
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

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        try
        {
            if (categoria is null)
                return BadRequest("Categoria é nula...");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao se comunicar com o servidor...");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        try
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest($"Categoria com id={id} não encontrada...");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao se comunicar com o servidor...");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao se comunicar com o servidor...");
        }
    }
}
