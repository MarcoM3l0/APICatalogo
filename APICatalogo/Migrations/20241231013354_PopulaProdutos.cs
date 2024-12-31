using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserindo produtos na categoria Bebidas
            migrationBuilder.Sql("INSERT into Produto(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaId) " + 
                "Values('Coca-Cola', 'Refrigerante de Cola 350 ml', 4.50, 'cocacola.jpg', 50, now(), 1)");
            migrationBuilder.Sql("INSERT into Produto(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaId) " +
                "Values('Pepsi', 'Refrigerante de Cola 350 ml', 4.00, 'pepsi.jpg', 50, now(), 1)");

            // Inserindo produtos na categoria Lanches
            migrationBuilder.Sql("INSERT INTO Produto(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaId) " +
                "Values('Sanduíche', 'Sanduíche de frango com salada', 12.50, 'sanduiche.jpg', 30, now(), 2)");

            migrationBuilder.Sql("INSERT INTO Produto(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaId) " +
                "Values('Hot Dog', 'Cachorro-quente com batata palha', 8.00, 'hotdog.jpg', 40, now(), 2)");

            // Inserindo produtos na categoria Sobremesas
            migrationBuilder.Sql("INSERT INTO Produto(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaId) " +
                "Values('Bolo de Chocolate', 'Bolo de chocolate com cobertura', 15.00, 'bolochocolate.jpg', 20, now(), 3)");
            migrationBuilder.Sql("INSERT INTO Produto(Nome, Descricao, Preco, ImagemURL, Estoque, DataCadastro, CategoriaId) " +
                "Values('Sorvete', 'Sorvete de baunilha com calda de chocolate', 10.00, 'sorvete.jpg', 25, now(), 3)");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Produto");
        }
    }
}
