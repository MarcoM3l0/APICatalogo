using APICatalogo.context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProtudosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProtudosController> _logger;
    private readonly IMapper _mapper;
    public ProtudosController(IUnitOfWork unitOfWork, ILogger<ProtudosController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {

        var produtos = _unitOfWork.ProdutosRepository.GetAll();

        if (produtos is null)
        {
            _logger.LogWarning("get - Produtos não encontrados");
            return NotFound("Produtos não encontrados...");
        }

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);

    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters) {

        var produtos = _unitOfWork.ProdutosRepository.GetProdutos(produtosParameters);

        if (produtos is null || !produtos.Any())
        {
            _logger.LogWarning("get - Produtos não encontrados com paginação");
            return NotFound("Produtos não encontrados...");
        }

        var produtosDtp = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDtp);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {

        var produto = _unitOfWork.ProdutosRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Get - Produto com id={id} não encontrado");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
       
    }

    [HttpGet("produto/{id}", Name = "ObterProdutosPorCategoria")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutosRepository.GetProdutoPorCategoria(id);

        if (produtos is null)
        {
            _logger.LogWarning($"Get - Produtos com categoria id={id} não encontrados");
            return NotFound($"Produtos com categoria id={id} não encontrados...");
        }

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }


    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
        {
            _logger.LogWarning("Post - Produto não informado");
            return BadRequest("Produto não informado...");
        }

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoCriado = _unitOfWork.ProdutosRepository.Create(produto);
        _unitOfWork.Commit();

        var produtoCriadoDto = _mapper.Map<ProdutoDTO>(produtoCriado);

        return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
        
    }

    [HttpPatch("{id:int}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
    {
        if(patchProdutoDto is null || id <= 0)
        {
            _logger.LogWarning($"Patch - Produto com id={id} não encontrado para atualização parcial");
            return BadRequest($"Produto com id={id} não encontrado...");
        }

        var produto = _unitOfWork.ProdutosRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Patch - Produto com id={id} não encontrado para atualização parcial");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
        patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

        if(!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
        {
            _logger.LogWarning($"Patch - Produto com id={id} não passou na validação do modelo");
            return BadRequest(ModelState);
        }

        _mapper.Map(produtoUpdateRequest, produto);

        _unitOfWork.ProdutosRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {

        if (id != produtoDto.ProdutoId)
        {
            _logger.LogWarning($"Put - Produto com id={id} não encontrado para atualização");
            return BadRequest($"Produto com id={id} não encontrado...");
        }

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoAtualizado = _unitOfWork.ProdutosRepository.Update(produto);
        _unitOfWork.Commit();

        var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _unitOfWork.ProdutosRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Delete - Produto com id={id} não encontrado para remoção");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoRemovido = _unitOfWork.ProdutosRepository.Delete(produto);
        _unitOfWork.Commit();

        var produtoRemovidoDto = _mapper.Map<ProdutoDTO>(produtoRemovido);

        return Ok(produtoRemovidoDto);

    }
}
