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

    private readonly IRepository<Categoria> _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoriasController> _logger;
    public CategoriasController(IRepository<Categoria> repository, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
            var categorias = _repository.GetAll();

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


        var categoria = _repository.GetById(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Get - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }
        return Ok(categoria);

    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {

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

        var categoria = _repository.GetById(c => c.CategoriaId == id);

        if(categoria is null)
        {
            _logger.LogWarning($"Delete - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _repository.Delete(categoria);

        return Ok(categoriaExcluida);
    }
}
