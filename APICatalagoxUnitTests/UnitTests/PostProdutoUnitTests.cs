using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalagoxUnitTests.UnitTests;
public class PostProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;
    public PostProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.logger, controller.mapper);
    }

    [Fact]
    public async Task PostProduto_Returns_CreatedStatusCode()
    {
        // Arrange
        var newProduto = new ProdutoDTO
        {
            Nome = "New Product",
            Descricao = "New Product Description",
            Preco = 100.00m,
            ImagemUrl = "http://example.com/image.jpg",
            CategoriaId = 2
        };

        // Act
        var data = await _controller.Post(newProduto);

        // Assert
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>(); // Verifica se o resultado é do tipo CreatedAtRouteResult
        createdResult.Subject.StatusCode.Should().Be(201); // Verifica se o status code do CreatedAtRouteResult é 201
    }

    [Fact]
    public async Task PostProduto_Return_BadRequest()
    {
        // Arrange
        ProdutoDTO produtoDTO = null; // ProdutoDTO nulo para simular BadRequest

        // Act
        var data = await _controller.Post(produtoDTO);

        // Assert
        var badRequestResult = data.Result.Should().BeOfType<BadRequestObjectResult>(); // Verifica se o resultado é do tipo BadRequestObjectResult
        badRequestResult.Subject.StatusCode.Should().Be(400); // Verifica se o status code do BadRequestObjectResult é 400
    }
}
