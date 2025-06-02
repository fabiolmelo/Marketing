using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketing.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstabelecimentoContatos",
                columns: table => new
                {
                    EstabelecimentoCnpj = table.Column<string>(type: "TEXT", nullable: false),
                    ContatoTelefone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstabelecimentoContatos", x => new { x.EstabelecimentoCnpj, x.ContatoTelefone });
                    table.ForeignKey(
                        name: "FK_EstabelecimentoContatos_Contatos_ContatoTelefone",
                        column: x => x.ContatoTelefone,
                        principalTable: "Contatos",
                        principalColumn: "Telefone",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstabelecimentoContatos_Estabelecimentos_EstabelecimentoCnpj",
                        column: x => x.EstabelecimentoCnpj,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstabelecimentoContatos_ContatoTelefone",
                table: "EstabelecimentoContatos",
                column: "ContatoTelefone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstabelecimentoContatos");
        }
    }
}
