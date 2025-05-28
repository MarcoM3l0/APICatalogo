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
    private readonly IRepository<Produto> _repository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<ProtudosController> _logger;
    public ProtudosController(IRepository<Produto> repository, ILogger<ProtudosController> logger, IProdutoRepository produtoRepository)
    {
        _repository = repository;
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {

        var produtos = _repository.GetAll();

        if (produtos is null)
        {
            _logger.LogWarning("get - Produtos não encontrados");
            return NotFound("Produtos não encontrados...");
        }

        return Ok(produtos);
        
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {

        var produto = _repository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Get - Produto com id={id} não encontrado");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        return produto;
       
    }

    [HttpGet("produto/{id}", Name = "ObterProdutosPorCategoria")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
    {
        var produtos = _produtoRepository.GetProdutoPorCategoria(id);

        if (produtos is null)
        {
            _logger.LogWarning($"Get - Produtos com categoria id={id} não encontrados");
            return NotFound($"Produtos com categoria id={id} não encontrados...");
        }

        return Ok(produtos);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
        {
            _logger.LogWarning("Post - Produto não informado");
            return BadRequest("Produto não informado...");
        }

        var produtoCriado = _repository.Create(produto);
        return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {

        if (id != produto.ProdutoId)
        {
            _logger.LogWarning($"Put - Produto com id={id} não encontrado para atualização");
            return BadRequest($"Produto com id={id} não encontrado...");
        }

        var produtoAtualizado = _repository.Update(produto);

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        var produto = _repository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Delete - Produto com id={id} não encontrado para remoção");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoRemovido = _repository.Delete(produto);

        return Ok(produtoRemovido);

    }
}
