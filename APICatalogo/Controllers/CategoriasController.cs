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
using Microsoft.Extensions.Caching.Memory;
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

    private readonly IMemoryCache _cache;
    private const string CacheCategoriasKey = "CategoriasCache";

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="CategoriasController"/>.
    /// </summary>
    /// <param name="unitOfWork">Unidade de trabalho para operações com o banco de dados</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    /// <param name="cache">Cache em memória para otimização de desempenho</param>
    public CategoriasController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger<CategoriasController> logger, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _cache = cache;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        if(!_cache.TryGetValue(CacheCategoriasKey, out IEnumerable<Categoria>? categorias))
        {
            categorias = await _unitOfWork.CategoriasRepository.GetAllAsync();

            if (categorias is null || !categorias.Any())
            {
                _logger.LogWarning("Get - Categorias não encontradas...");
                return NotFound("Categorias não encontradas...");
            }
            
            SetCache(CacheCategoriasKey, categorias);
        }

        var categoriasDto = categorias?.ToCategoriaDtoList();

        return Ok(categoriasDto);
    }

    /// <summary>
    /// Obtém uma categoria pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Objeto Categoria se encontrado, ou NotFound se não encontrado</returns>
    [DisableCors]
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        //throw new Exception("Erro ao buscar categoria..."); // Simulando erro para teste do middleware
        //throw new ArgumentException("Ocorreu um erro no tratamento de request"); // Simulando erro para teste

        var cacheKey = GetCacheCategoriasKey(id);

        if(!_cache.TryGetValue(cacheKey, out Categoria? categoria))
        {
            categoria = await _unitOfWork.CategoriasRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is  null)
            {
                _logger.LogWarning("Get - Categorias não encontradas...");
                return NotFound("Categorias não encontradas...");
            }

            SetCache(cacheKey, categoria);
        }

        var categoriaDto = categoria?.ToCategoriaDTO();

        return Ok(categoriaDto);

    }

    /// <summary>
    /// Obtém uma lista de categorias com paginação
    /// </summary>
    /// <param name="categoriasParameters">Este parâmetro é usado para definir a paginação e filtragem das categorias.</param>
    /// <returns>Retorna uma lista de objetos CategoriaDTO paginados se encontrados, ou NotFound se não encontrados.</returns>
    [HttpGet("pagination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        string cacheKey = GetCacheCategoriasKey(categoriasParameters);

        if (!_cache.TryGetValue(cacheKey, out PagedList<Categoria>? categorias))
        {
            categorias = await _unitOfWork.CategoriasRepository.GetCategoriasAsync(categoriasParameters);
            if (categorias is null || !categorias.Any())
            {
                _logger.LogWarning("Get - Categorias não encontradas...");
                return NotFound("Categorias não encontradas...");
            }

            SetCache(cacheKey, categorias);
        }

        return ObterCategorias(categorias);
    }

    /// <summary>
    /// Obtém uma lista de categorias filtradas por nome com paginação
    /// </summary>
    /// <param name="categoriasFiltroNome">Este parâmetro é usado para definir a filtragem por nome e paginação das categorias.</param>
    /// <returns>Retorna uma lista de objetos CategoriaDTO filtrados por nome e paginados se encontrados, ou NotFound se não encontrados.</returns>
    [HttpGet("filtro/nome/pagination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaFiltroNome([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
    {
        string cacheKey = GetCacheCategoriasKey(categoriasFiltroNome);

        if (!_cache.TryGetValue(cacheKey, out PagedList<Categoria>? categorias))
        {
            categorias = await _unitOfWork.CategoriasRepository.GetCategoriasFiltroNomeAsync(categoriasFiltroNome);
            if (categorias is null || !categorias.Any())
            {
                _logger.LogWarning("GetCategoriaFiltroNome - Categorias não encontradas com filtro de nome");
                return NotFound("Categorias não encontradas...");
            }

            SetCache(cacheKey, categorias);
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        InvalidateCacheAfterChange(categoriaDtoCriada.CategoriaId, categoriaCriada);

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        InvalidateCacheAfterChange(id, categoria);

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        InvalidateCacheAfterChange(id);

        return Ok(categoriaDtoExcluida);
    }

    private static string GetCacheCategoriasKey(int id) => $"CategoriasCache_{id}";
    private static string GetCacheCategoriasKey(CategoriasParameters categoriasParameters) => $"CategoriasCache_{categoriasParameters.PageNumber}_{categoriasParameters.PageSize}";
    private static string GetCacheCategoriasKey(CategoriasFiltroNome categoriasFiltroNome) => $"CategoriasCache_FiltroNome_{categoriasFiltroNome.Nome}_{categoriasFiltroNome.PageNumber}_{categoriasFiltroNome.PageSize}";

    private void SetCache<T>(string key, T data)
    {
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
            SlidingExpiration = TimeSpan.FromSeconds(30),
            Priority = CacheItemPriority.High
        };
        _cache.Set(key, data, cacheOptions);
    }

    private void InvalidateCacheAfterChange(int id, Categoria? categoria = null)
    {
        _cache.Remove(CacheCategoriasKey);
        _cache.Remove(GetCacheCategoriasKey(id));

        if (categoria is not null)
        {
            SetCache(GetCacheCategoriasKey(id), categoria);
        }
    }

}
