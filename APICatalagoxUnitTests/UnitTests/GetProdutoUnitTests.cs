using APICatalogo.Controllers;
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
}
