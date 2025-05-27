using APICatalogo.context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProtudosController : ControllerBase
{
    private readonly IProdutoRepository _repository;
    private readonly ILogger<ProtudosController> _logger;
    public ProtudosController(IProdutoRepository repository, ILogger<ProtudosController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        _logger.LogInformation("Get - produtos");

        var produtos = _repository.GetProdutos().AsNoTracking().ToList();

        if (produtos is null)
        {
            _logger.LogWarning("get - Produtos não encontrados");
            return NotFound("Produtos não encontrados...");
        }

        return produtos;
        
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        _logger.LogInformation($"Get - produto/id={id}");

        var produto = _repository.GetProduto(id);

        if (produto is null)
        {
            _logger.LogWarning($"Get - Produto com id={id} não encontrado");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        return produto;
       
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        _logger.LogInformation("Post - produto");
        
        var produtoCriado = _repository.Create(produto);
        return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        _logger.LogInformation($"Put - produto/id={id}");

        if (id != produto.ProdutoId)
        {
            _logger.LogWarning($"Put - Produto com id={id} não encontrado para atualização");
            return BadRequest($"Produto com id={id} não encontrado...");
        }

        bool produtoAtualizado = _repository.Update(produto);

        if (!produtoAtualizado)
        {
            _logger.LogWarning($"Put - Produto com id={id} não poder ser atualizado");
            return StatusCode(500, $"Produto com id={id} não encontrado...");
        }

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        _logger.LogInformation($"Delete - produto/id={id}");

        bool produtoRemovido = _repository.Delete(id);

        if (!produtoRemovido)
        {
            _logger.LogWarning($"Delete - Produto com id={id} não poder ser removido");
            return StatusCode(500, $"Falha ao remover produto com id={id}...");
        }

        return Ok($"Produto com id={id} removido com sucesso...");

    }
}
