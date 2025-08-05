using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

/// <summary>
/// Controlador responsável por operações CRUD de produtos.
/// </summary>
/// <remarks>
/// Este controlador fornece endpoints para:
/// - Listar todos os produtos
/// - Obter produtos paginados
/// - Filtrar produtos por preço
/// - Buscar produtos por categoria
/// - Criar, atualizar e excluir produtos
/// 
/// Requer autenticação para algumas operações conforme políticas de segurança definidas.
/// </remarks>
[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProdutosController> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ProdutosController"/>.
    /// </summary>
    /// <param name="unitOfWork">Unidade de trabalho para operações com o banco de dados</param>
    /// <param name="logger">Logger para registro de eventos e monitoramento</param>
    /// <param name="mapper">Mapeador para conversão entre DTOs e entidades</param>
    public ProdutosController(IUnitOfWork unitOfWork, ILogger<ProdutosController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDtp = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDtp);
    }

    /// <summary>
    /// Efetua a consulta de todos os produtos
    /// </summary>
    /// <returns>Retorna uma lista de objetos Produto se encontrados, ou NotFound se não encontrados.</returns>
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
    {

        var produtos = await _unitOfWork.ProdutosRepository.GetAllAsync();

        if (produtos is null)
        {
            _logger.LogWarning("get - Produtos não encontrados");
            return NotFound("Produtos não encontrados...");
        }

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);

    }

    /// <summary>
    /// Recupera uma lista de produtos com paginação
    /// </summary>
    /// <param name="produtosParameters">Parametros de paginação para filtrar e ordenar os produtos.</param>
    /// <returns>Retorna uma lista de objetos ProdutoDTO com paginação, ou NotFound se não encontrados.</returns>
    [HttpGet("pagination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
    {

        var produtos = await _unitOfWork.ProdutosRepository.GetProdutosAsync(produtosParameters);

        if (produtos is null || !produtos.Any())
        {
            _logger.LogWarning("get - Produtos não encontrados com paginação");
            return NotFound("Produtos não encontrados...");
        }

        return ObterProdutos(produtos);
    }

    /// <summary>
    /// Recupera uma lista de produtos filtrados por preço com paginação
    /// </summary>
    /// <param name="produtosFiltroParameters">Parametros de filtro de preço para filtrar e ordenar os produtos.</param>
    /// <returns>Retorna uma lista de objetos ProdutoDTO filtrados por preço com paginação, ou NotFound se não encontrados.</returns>
    [HttpGet("filtro/preco/pagination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorPreco([FromQuery] ProdutosFiltroPreco produtosFiltroParameters)
    {
        var produtos = await _unitOfWork.ProdutosRepository.GetProdutosFiltroPrecoAsync(produtosFiltroParameters);

        if (produtos is null || !produtos.Any())
        {
            _logger.LogWarning("get - Produtos não encontrados com filtro de preço");
            return NotFound("Produtos não encontrados...");
        }

        return ObterProdutos(produtos);
    }

    /// <summary>
    /// Obtém um produto pelo id
    /// </summary>
    /// <param name="id">Parametro id e o identificador único do produto que será consultado.</param>
    /// <returns>Um objeto Produto se encontrado, ou NotFound se não encontrado.</returns>
    [HttpGet("{id:int}", Name = "ObterProduto")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {

        var produto = await _unitOfWork.ProdutosRepository.GetAsync(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Get - Produto com id={id} não encontrado");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
       
    }

    /// <summary>
    /// Recupera uma lista de produtos por categoria
    /// </summary>
    /// <param name="id">Parametro id e o identificador único da categoria que será consultada.</param>
    /// <returns>Uma lista de objetos ProdutoDTO filtrados por categoria se encontrados, ou NotFound se não encontrados.</returns>
    [HttpGet("produto/{id}", Name = "ObterProdutosPorCategoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorCategoria(int id)
    {
        var produtos = await _unitOfWork.ProdutosRepository.GetProdutoPorCategoriaAsync(id);

        if (produtos is null || !produtos.Any())
        {
            _logger.LogWarning($"Get - Produtos com categoria id={id} não encontrados");
            return NotFound($"Produtos com categoria id={id} não encontrados...");
        }

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     POST /produtos
    ///     {
    ///         "nome": "Produto Exemplo",
    ///         "descricao": "Descrição do produto exemplo",
    ///         "preco": 99.99,
    ///         "imagemUrl": "http://example.com/imagem.jpg",
    ///         "categoriaId": 1
    ///     }
    /// </remarks>
    /// <param name="produtoDto"></param>
    /// <returns>Retorna o status 201 Created com o objeto ProdutoDTO criado, ou BadRequest se o produto não for informado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
        {
            _logger.LogWarning("Post - Produto não informado");
            return BadRequest("Produto não informado...");
        }

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoCriado = _unitOfWork.ProdutosRepository.Create(produto);
        await _unitOfWork.CommitAsync();

        var produtoCriadoDto = _mapper.Map<ProdutoDTO>(produtoCriado);

        return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        
    }

    /// <summary>
    /// Parcialmente atualiza um produto
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     PATCH /produtos/1/UpdatePartial
    ///     {
    ///         "produtoId": 1,
    ///         "nome": "Produto atual",
    ///         "descricao": "Descrição do produto sem ser atualizado",
    ///         "preco": 89.99,
    ///         "imagemUrl": "http://example.com/imagem-atual.jpg",
    ///         "categoriaId": 1
    ///     }
    /// </remarks>
    /// <param name="id">Parametro id e o identificador único do produto que será atualizado.</param>
    /// <param name="patchProdutoDto">PatchDocument que contém as operações de atualização parcial a serem aplicadas ao produto.</param>
    /// <returns>
    /// Retorna o status 200 OK com o objeto ProdutoDTO atualizado, ou BadRequest se o produto não for informado ou se o id for inválido.
    /// </returns>
    [HttpPatch("{id:int}/UpdatePartial")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
    {
        if(patchProdutoDto is null || id <= 0)
        {
            _logger.LogWarning($"Patch - Produto com id={id} não encontrado para atualização parcial");
            return BadRequest($"Produto com id={id} não encontrado...");
        }

        var produto = await _unitOfWork.ProdutosRepository.GetAsync(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Patch - Produto com id={id} não encontrado para atualização parcial");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

        if(!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
        {
            _logger.LogWarning($"Patch - Produto com id={id} não passou na validação do modelo");
            return BadRequest(ModelState);
        }

        _mapper.Map(produtoUpdateRequest, produto);

        _unitOfWork.ProdutosRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    /// <summary>
    /// Atualiza um produto existente
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     PUT /produtos/1
    ///     {
    ///         "produtoId": 1,
    ///         "nome": "Produto atualizado",
    ///         "descricao": "Descrição do produto atualizado",
    ///         "preco": 89.99,
    ///         "imagemUrl": "http://example.com/imagem-atualizada.jpg",
    ///         "categoriaId": 1
    ///     }
    /// </remarks>
    /// <param name="id">Parametro id e o identificador único do produto que será atualizado.</param>
    /// <param name="produtoDto">
    /// Parametro produtoDto é o objeto ProdutoDTO que contém as informações atualizadas do produto.
    /// </param>
    /// <returns>
    /// Retorna o status 200 OK com o objeto ProdutoDTO atualizado, ou BadRequest se o id não corresponder ao produtoDto ou se o produto não for encontrado.
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
    {

        if (id != produtoDto.ProdutoId)
        {
            _logger.LogWarning($"Put - Produto com id={id} não encontrado para atualização");
            return BadRequest($"Produto com id={id} não encontrado...");
        }

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoAtualizado = _unitOfWork.ProdutosRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDto);
    }

    /// <summary>
    /// Exclui um produto pelo id
    /// </summary>
    /// <param name="id">Parametro id é o identificador único do produto que será removido.</param>
    /// <returns>Retorna o status 200 OK com o objeto ProdutoDTO removido, ou NotFound se o produto não for encontrado.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _unitOfWork.ProdutosRepository.GetAsync(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Delete - Produto com id={id} não encontrado para remoção");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoRemovido = _unitOfWork.ProdutosRepository.Delete(produto);
        await _unitOfWork.CommitAsync();

        var produtoRemovidoDto = _mapper.Map<ProdutoDTO>(produtoRemovido);

        return Ok(produtoRemovidoDto);

    }
}
