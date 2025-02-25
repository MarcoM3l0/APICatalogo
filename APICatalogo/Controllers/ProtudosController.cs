using APICatalogo.context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProtudosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProtudosController(AppDbContext contexto)
    {
        _context = contexto;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        try
        {
            var produtos = await _context.Produtos.ToListAsync();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }

            return produtos;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter os produtos do banco de dados...");
        }
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> Get(int id)
    {
        try
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto is null)
            {
                return NotFound($"Produto com id={id} não encontrado...");
            }
            return produto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter o produto do banco de dados...");
        }
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        try
        {
            if (produto is null)
                return BadRequest("Produto é nulo...");

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar criar um novo produto...");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        try
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest($"Produto com id={id} não encontrado...");
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok($"Produto com id={id} foi atualizado com sucesso...");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar o produto...");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        try
        {
            var produto = _context.Produtos.Find(id);
            if (produto is null)
            {
                return NotFound($"Produto com id={id} não encontrado...");
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar deletar o produto...");
        }
    }
}
