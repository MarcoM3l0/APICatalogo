using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;


/// <summary>
/// Controlador responsável por gerenciar operações relacionadas a categorias.
/// </summary>
/// <remarks>
/// Este controlador fornece endpoints para:
/// - Listar categorias (com paginação e filtros)
/// - Obter categorias por ID
/// - Criar novas categorias
/// - Atualizar categorias existentes
/// - Excluir categorias
/// </remarks>
[EnableCors("CorsPolicy")]
[EnableRateLimiting("fixedwindow")]
[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CategoriasController> _logger;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="CategoriasController"/>.
    /// </summary>
    /// <param name="unitOfWork">Unidade de trabalho para operações com o banco de dados</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public CategoriasController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<CategoriasController> logger)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }
    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
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


    /// <summary>
    /// Obtém todas os objetos de categoria
    /// </summary>
    /// <returns>Objetos Categoria se encontrados, ou NotFound se não encontrados</returns>
    [Authorize]
    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    [DisableRateLimiting]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        var categorias = await _unitOfWork.CategoriasRepository.GetAllAsync();

        if (categorias is null)
        {
            _logger.LogWarning("Get - Categorias não encontradas...");
            return NotFound("Categorias não encontradas...");
        }

        var categoriasDto = categorias.ToCategoriaDtoList();

        return Ok(categoriasDto);
    }

    /// <summary>
    /// Obtém uma categoria pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Objeto Categoria se encontrado, ou NotFound se não encontrado</returns>
    [DisableCors]
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        //throw new Exception("Erro ao buscar categoria..."); // Simulando erro para teste do middleware
        //throw new ArgumentException("Ocorreu um erro no tratamento de request"); // Simulando erro para teste


        var categoria = await _unitOfWork.CategoriasRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Get - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaDto = categoria.ToCategoriaDTO();

        return Ok(categoriaDto);

    }

    /// <summary>
    /// Obtém uma lista de categorias com paginação
    /// </summary>
    /// <param name="categoriasParameters">Este parâmetro é usado para definir a paginação e filtragem das categorias.</param>
    /// <returns>Retorna uma lista de objetos CategoriaDTO paginados se encontrados, ou NotFound se não encontrados.</returns>
    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _unitOfWork.CategoriasRepository.GetCategoriasAsync(categoriasParameters);

        if (categorias is null || !categorias.Any())
        {
            _logger.LogWarning("Get - Categorias não encontradas com paginação");
            return NotFound("Categorias não encontradas...");
        }

        return ObterCategorias(categorias);
    }

    /// <summary>
    /// Obtém uma lista de categorias filtradas por nome com paginação
    /// </summary>
    /// <param name="categoriasFiltroNome">Este parâmetro é usado para definir a filtragem por nome e paginação das categorias.</param>
    /// <returns>Retorna uma lista de objetos CategoriaDTO filtrados por nome e paginados se encontrados, ou NotFound se não encontrados.</returns>
    [HttpGet("filtro/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaFiltroNome([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
    {
        var categorias = await _unitOfWork.CategoriasRepository.GetCategoriasFiltroNomeAsync(categoriasFiltroNome);

        if (categorias is null || !categorias.Any())
        {
            _logger.LogWarning("GetCategoriaFiltroNome - Categorias não encontradas com filtro de nome");
            return NotFound("Categorias não encontradas...");
        }

        return ObterCategorias(categorias);
    }

    /// <summary>
    /// Inclui uma nova categoria
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     POST /categorias
    ///     {
    ///         "CategoriaId": 1,
    ///         "Nome": "Categoria Teste",
    ///         "imagemUrl": "https://example.com/imagem.jpg"
    ///     }
    /// </remarks>
    /// <param name="categoriaDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
    {

        if (categoriaDto is null)
        {
            _logger.LogWarning("Post - Categoria é nula");
            return BadRequest("Categoria é nula...");
        }

        var categoria = categoriaDto.ToCategoria();

        if (categoria is null)
        {
            _logger.LogWarning("Post - Categoria não pôde ser convertida de CategoriaDTO");
            return BadRequest("Categoria não pôde ser convertida de CategoriaDTO...");
        }

        var categoriaCriada = _unitOfWork.CategoriasRepository.Create(categoria);
        await _unitOfWork.CommitAsync();

        var categoriaDtoCriada = categoriaCriada.ToCategoriaDTO();

        if (categoriaDtoCriada is null)
        {
            _logger.LogWarning("Post - CategoriaDTO criada é nula");
            return BadRequest("CategoriaDTO criada é nula...");
        }

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDtoCriada.CategoriaId }, categoriaDtoCriada);
        
    }

    /// <summary>
    /// Atualiza uma categoria existente
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     PUT /categorias/1
    ///     {
    ///         "CategoriaId": 1,
    ///         "Nome": "Categoria Atualizada",
    ///         "imagemUrl": "https://example.com/imagem-atualizada.jpg"
    ///     }
    /// </remarks>
    /// <param name="id">Parametro id é o identificador da categoria a ser atualizada.</param>
    /// <param name="categoriaDto">Parametro categoriaDto é o objeto que contém os dados atualizados da categoria.</param>
    /// <returns>
    /// Retorna o objeto CategoriaDTO atualizado se a atualização for bem-sucedida, ou BadRequest se o id não confere com o id do objeto.
    /// </returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
    {

        if (id != categoriaDto.CategoriaId)
        {
            _logger.LogWarning($"Put - Id da categoria ({id}) não confere com o id do objeto ({categoriaDto.CategoriaId})");
            return BadRequest($"Categoria com id={id} não encontrada...");
        }

        var categoria = categoriaDto.ToCategoria();

        if(categoria is null)
        {
            _logger.LogWarning("Put - Categoria é nula");
            return BadRequest("Categoria é nula...");
        }

        _unitOfWork.CategoriasRepository.Update(categoria);
        await _unitOfWork.CommitAsync();

        var categoriaDtoAtualizada = categoria.ToCategoriaDTO();

        return Ok(categoriaDtoAtualizada);
        
    }

    /// <summary>
    /// Exclui uma categoria pelo id
    /// </summary>
    /// <param name="id">Parametro id é o identificador da categoria a ser excluída.</param>
    /// <returns>
    /// Retorna o objeto CategoriaDTO excluído se a exclusão for bem-sucedida, ou NotFound se a categoria não for encontrada.
    /// </returns>
    [Authorize(policy:"AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {

        var categoria = await _unitOfWork.CategoriasRepository.GetAsync(c => c.CategoriaId == id);

        if(categoria is null)
        {
            _logger.LogWarning($"Delete - Categoria com id={id} não encontrada");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _unitOfWork.CategoriasRepository.Delete(categoria);
        await _unitOfWork.CommitAsync();

        var categoriaDtoExcluida = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaDtoExcluida);
    }
}
