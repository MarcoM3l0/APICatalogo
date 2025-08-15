using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalagoxUnitTests.UnitTests;
public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.logger, controller.mapper);
    }

    [Fact]
    public async Task DeleteProdutoById_Returns_OkResult()
    {
        // Arrange
        var ProdutoId = 15;

        // Act
        var result = await _controller.Delete(ProdutoId);

        // Assert
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é do tipo OkObjectResult
    }

    [Fact]
    public async Task DeleteProdutoById_Returns_NotFoundResult()
    {
        // Arrange
        var ProdutoId = 1; // ID que não existe
        // Act
        var result = await _controller.Delete(ProdutoId) as ActionResult<ProdutoDTO>;
        // Assert
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<NotFoundObjectResult>(); // Verifica se o status code do NotFoundObjectResult é 404
    }
}
