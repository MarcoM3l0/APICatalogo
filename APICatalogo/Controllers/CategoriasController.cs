using APICatalogo.context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoriasController> _logger;
    public CategoriasController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }
    private ActionResult<IEnumerable<CategoriaDTO>> obterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDto = categorias.ToCategoriaDtoList();
        return Ok(categoriasDto);
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _unitOfWork.CategoriasRepository.GetAll();

        if (categorias is null)
        {
            _logger.LogWarning("Get - Categorias não encontradas...");
            return NotFound("Categorias não encontradas...");
        }

        var categoriasDto = categorias.ToCategoriaDtoList();

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        //throw new Exception("Erro ao buscar categoria..."); // Simulando erro para teste do middleware
        //throw new ArgumentException("Ocorreu um erro no tratamento de request"); // Simulando erro para teste


        var categoria = _unitOfWork.CategoriasRepository.GetById(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Get - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaDto = categoria.ToCategoriaDTO();

        return Ok(categoriaDto);

    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = _unitOfWork.CategoriasRepository.GetCategorias(categoriasParameters);

        if (categorias is null || !categorias.Any())
        {
            _logger.LogWarning("Get - Categorias não encontradas com paginação");
            return NotFound("Categorias não encontradas...");
        }

        return obterCategorias(categorias);
    }

    [HttpGet("filtro/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriaFiltroNome([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
    {
        var categorias = _unitOfWork.CategoriasRepository.GetCategoriasFiltroNome(categoriasFiltroNome);

        if (categorias is null || !categorias.Any())
        {
            _logger.LogWarning("GetCategoriaFiltroNome - Categorias não encontradas com filtro de nome");
            return NotFound("Categorias não encontradas...");
        }

        return obterCategorias(categorias);
    }


    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {

        if (categoriaDto is null)
        {
            _logger.LogWarning("Post - Categoria é nula");
            return BadRequest("Categoria é nula...");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _unitOfWork.CategoriasRepository.Create(categoria);
        _unitOfWork.Commit();

        var categoriaDtoCriada = categoriaCriada.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDtoCriada.CategoriaId }, categoriaDtoCriada);
        
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {

        if (id != categoriaDto.CategoriaId)
        {
            _logger.LogWarning($"Put - Id da categoria ({id}) não confere com o id do objeto ({categoriaDto.CategoriaId})");
            return BadRequest($"Categoria com id={id} não encontrada...");
        }

        var categoria = categoriaDto.ToCategoria();

        _unitOfWork.CategoriasRepository.Update(categoria);
        _unitOfWork.Commit();

        var categoriaDtoAtualizada = categoria.ToCategoriaDTO();

        return Ok(categoriaDtoAtualizada);
        
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {

        var categoria = _unitOfWork.CategoriasRepository.GetById(c => c.CategoriaId == id);

        if(categoria is null)
        {
            _logger.LogWarning($"Delete - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _unitOfWork.CategoriasRepository.Delete(categoria);
        _unitOfWork.Commit();

        var categoriaDtoExcluida = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaDtoExcluida);
    }
}
