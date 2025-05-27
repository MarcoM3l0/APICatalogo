using APICatalogo.context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{

    private readonly ICategoriaRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoriasController> _logger;
    public CategoriasController(ICategoriaRepository repository, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
            _logger.LogInformation("Get - categoria");

            var categorias = _repository.GetCategorias();

            if (categorias is null)
            {
                _logger.LogWarning("Get - Categorias não encontradas...");
                return NotFound("Categorias não encontradas...");
            }

            return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        //throw new Exception("Erro ao buscar categoria..."); // Simulando erro para teste do middleware
        //throw new ArgumentException("Ocorreu um erro no tratamento de request"); // Simulando erro para teste


        var categoria = _repository.GetCategoria(id);

        _logger.LogInformation($"Get - categoria/id={id}");

        if (categoria is null)
        {
            _logger.LogWarning("Get - Categoria não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }
        return Ok(categoria);

    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        _logger.LogInformation("Get - categoria/produto");

        var categorias = _repository.GetCategoriasProdutos();
        if (categorias is null)
        {
            _logger.LogWarning("Get - Produtos por categorias não encontradas");
            return NotFound("Categorias não encontradas...");
        }
        return Ok(categorias);

    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        _logger.LogInformation("Post - categoria");

        if (categoria is null)
        {
            _logger.LogWarning("Post - Categoria é nula");
            return BadRequest("Categoria é nula...");
        }

        var categoriaCriada = _repository.Create(categoria);
        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        _logger.LogInformation($"Put - categoria/id={id}");

        if (id != categoria.CategoriaId)
        {
            _logger.LogWarning($"Put - Id da categoria ({id}) não confere com o id do objeto ({categoria.CategoriaId})");
            return BadRequest($"Categoria com id={id} não encontrada...");
        }

        _repository.Update(categoria);

        return Ok(categoria);
        
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Categoria> Delete(int id)
    {
        _logger.LogInformation($"Delete - categoria/id={id}");

        var categoria = _repository.GetCategoria(id);

        if(categoria is null)
        {
            _logger.LogWarning($"Delete - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _repository.Delete(id);

        return Ok(categoriaExcluida);
    }
}
