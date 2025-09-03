using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketing.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Inicial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataRecusa",
                table: "Contatos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAceite",
                table: "Contatos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Mensagens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ContatoFone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ContatoTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    Competencia = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensagens_Contatos_ContatoTelefone",
                        column: x => x.ContatoTelefone,
                        principalTable: "Contatos",
                        principalColumn: "Telefone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensagemEventos",
                columns: table => new
                {
                    MensagemId = table.Column<string>(type: "TEXT", nullable: false),
                    MensagemStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    DataEvento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensagemEventos", x => new { x.MensagemId, x.MensagemStatus });
                    table.ForeignKey(
                        name: "FK_MensagemEventos_Mensagens_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_ContatoTelefone",
                table: "Mensagens",
                column: "ContatoTelefone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MensagemEventos");

            migrationBuilder.DropTable(
                name: "Mensagens");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataRecusa",
                table: "Contatos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAceite",
                table: "Contatos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
