using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalagoxUnitTests.UnitTests;
public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.logger, controller.mapper);
    }

    [Fact]
    public async Task PutProduto_Returns_OkResult()
    {
        // Arrange
        var produtoId = 15; // ID do produto a ser atualizado
        var produtoDto = new ProdutoDTO 
        { 
            ProdutoId = produtoId, 
            Nome = "Produto Atualizado",
            Descricao = "Descrição do Produto Atualizado",
            ImagemUrl = "https://example.com/imagem-atualizada.jpg",
            CategoriaId = 1
        };

        // Act
        var result = await _controller.Put(produtoId, produtoDto) as ActionResult<ProdutoDTO>;
        // Assert
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é do tipo OkObjectResult
    }

    [Fact]
    public async Task PutProduto_Returns_BadRequest()
    {
        // Arrange
        var produtoId = 100; // ID do produto a ser atualizado
        var produtoDto = new ProdutoDTO 
        { 
            ProdutoId = 15, 
            Nome = "Produto Atualizado",
            Descricao = "Descrição do Produto Atualizado",
            ImagemUrl = "https://example.com/imagem-atualizada.jpg",
            CategoriaId = 1
        };

        // Act
        var data = await _controller.Put(produtoId, produtoDto); // Passando um ID diferente

        // Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    }
}
