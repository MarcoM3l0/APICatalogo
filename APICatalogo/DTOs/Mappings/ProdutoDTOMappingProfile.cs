using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings;

/// <summary>
/// Classe de configuração de mapeamento entre entidades e DTOs usando AutoMapper
/// </summary>
/// <remarks>
/// Define os mapeamentos entre:
/// - Produto e ProdutoDTO (e vice-versa)
/// - Produto e ProdutoDTOUpdateRequest (e vice-versa)
/// - Produto e ProdutoDTOUpdateResponse (e vice-versa)
/// - Categoria e CategoriaDTO (e vice-versa)
/// </remarks>
public class ProdutoDTOMappingProfile : Profile
{
    /// <summary>
    /// Configura os mapeamentos entre entidades e DTOs
    /// </summary>
    public ProdutoDTOMappingProfile()
    {
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
    }
}
