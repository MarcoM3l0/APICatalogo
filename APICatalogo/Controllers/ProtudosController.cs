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
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _context.Produtos.ToList();

        if(produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        return produtos;
    }

    [HttpGet("{id:int}", Name= "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _context.Produtos.Find(id);
        if (produto is null)
        {
            return NotFound($"Produto com id={id} não encontrado...");
        }
        return produto;
    }

    [HttpPost]
    public ActionResult Post( Produto produto)
    {
        if (produto is null)
            return BadRequest("Produto é nulo...");

        _context.Produtos.Add(produto);
        _context.SaveChanges();
        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest($"Produto com id={id} não encontrado...");
        }
        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
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
}
