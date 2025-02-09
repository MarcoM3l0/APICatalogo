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
    public CategoriasController(AppDbContext contexto)
    {
        _context = contexto;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {

        try
        {
            var categorias = _context.Categorias.ToList();

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
    public ActionResult<Categoria> Get(int id)
    {
        try
        {
            var categoria = _context.Categorias.Find(id);
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
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        try
        {
            var categorias = _context.Categorias.Include(x => x.Produtos).ToList();
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
