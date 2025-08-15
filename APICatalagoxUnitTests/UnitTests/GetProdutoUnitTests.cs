using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Pagination;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalagoxUnitTests.UnitTests;
public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.logger, controller.mapper);
    }

    [Fact]
    public async Task GetProdutos_Returns_ListOfProdutoDTO()
    {
        // Act
        var data = await _controller.Get();
        // Assert
        data.Result.Should().BeOfType<OkObjectResult>() // Verifica se o resultado é do tipo OkObjectResult
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>() //Verifica se o valor do OkObjectResult é atribuído ao tipo IEnumerable<ProdutoDTO>
            .And.NotBeNull(); // Verifica se o valor não é nulo
    }

    [Fact]
    public async Task GetProdutoByID_Returns_OkResult()
    {
        // Arrange
        var ProdutoId = 15;

        // Act
        var data = await _controller.Get(ProdutoId);

        //Assert(xUnit)
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(FluentAssertions)
        data.Result.Should().BeOfType<OkObjectResult>() // Verifica se o resultado é do tipo OkObjectResult
            .Which.StatusCode.Should().Be(200); // Verifica se o status code do OkObjectResult é 200
    }

    [Fact]
    public async Task GetProdutoByID_Returns_NotFoundResult()
    {
        // Arrange
        var ProdutoId = 1; // ID que não existe

        // Act
        var data = await _controller.Get(ProdutoId);

        // Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>() // Verifica se o resultado é do tipo NotFoundObjectResult
            .Which.StatusCode.Should().Be(404); // Verifica se o status code do NotFoundObjectResult é 404
    }

    [Fact]
    public async Task GetProdutoByCategoria_OkResult()
    {
        // Arrange
        var categoria = 1;
        // Act
        var data = await _controller.GetProdutosPorCategoria(categoria);
        // Assert
        data.Result.Should().BeOfType<OkObjectResult>() // Verifica se o resultado é do tipo OkObjectResult
            .Which.StatusCode.Should().Be(200); // Verifica se o status code do OkObjectResult é 200
    }

    [Fact]
    public async Task GetProdutoByCategoria_NotFoundResult()
    {
        // Arrange
        var categoriaID = 99999; // Categoria que não existe
        // Act
        var data = await _controller.GetProdutosPorCategoria(categoriaID);
        // Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>() // Verifica se o resultado é do tipo NotFoundObjectResult
            .Which.StatusCode.Should().Be(404); // Verifica se o status code do NotFoundObjectResult é 404
    }
}
