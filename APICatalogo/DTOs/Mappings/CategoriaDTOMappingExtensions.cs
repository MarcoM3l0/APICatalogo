using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings;

/// <summary>
/// Classe de extensão para mapeamento entre entidades Categoria e DTOs relacionados
/// </summary>
public static class CategoriaDTOMappingExtensions
{
    /// <summary>
    /// Converte uma entidade Categoria para CategoriaDTO
    /// </summary>
    /// <param name="categoria">Entidade Categoria a ser convertida</param>
    /// <returns>
    /// Retorna um CategoriaDTO se a conversão for bem-sucedida,
    /// ou null se o parâmetro categoria for null
    /// </returns>
    public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
    {
        if (categoria is null)  return null;

        return new CategoriaDTO
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
    }

    /// <summary>
    /// Converte um CategoriaDTO para entidade Categoria
    /// </summary>
    /// <param name="categoriaDto">DTO CategoriaDTO a ser convertido</param>
    /// <returns>
    /// Retorna uma entidade Categoria se a conversão for bem-sucedida,
    /// ou null se o parâmetro categoriaDto for null
    /// </returns>
    public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null) return null;

        return new Categoria
        {
            CategoriaId = categoriaDto.CategoriaId,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };
    }

    /// <summary>
    /// Converte uma coleção de entidades Categoria para uma lista de CategoriaDTO
    /// </summary>
    /// <param name="categorias">Coleção de entidades Categoria a serem convertidas</param>
    /// <returns>
    /// Retorna uma lista de CategoriaDTO se a coleção contiver elementos,
    /// ou uma lista vazia se o parâmetro categorias for null ou vazio
    /// </returns>
    public static IEnumerable<CategoriaDTO> ToCategoriaDtoList(this IEnumerable<Categoria> categorias)
    {
        if (categorias is null || !categorias.Any()) return new List<CategoriaDTO>();

        return categorias.Select(
            categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            }).ToList();
    }
}
