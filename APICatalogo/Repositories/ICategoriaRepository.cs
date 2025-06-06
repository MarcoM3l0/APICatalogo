﻿using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface ICategoriaRepository :IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
    PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroParams);
}
