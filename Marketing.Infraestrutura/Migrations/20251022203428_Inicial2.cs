using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketing.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Inicial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Estabelecimentos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Estabelecimentos");
        }
    }
}
