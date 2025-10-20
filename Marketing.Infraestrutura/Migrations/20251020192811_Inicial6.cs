using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketing.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Inicial6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MetaApiUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MetaToken = table.Column<string>(type: "TEXT", nullable: true),
                    FoneFrom = table.Column<string>(type: "TEXT", nullable: true),
                    LoteProcessamento = table.Column<int>(type: "INTEGER", nullable: true),
                    IntervaloEntreDisparos = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");
        }
    }
}
