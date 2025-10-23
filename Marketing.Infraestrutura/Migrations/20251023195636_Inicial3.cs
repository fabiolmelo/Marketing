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
            migrationBuilder.DropPrimaryKey(
                name: "PK_EnviosMensagemMensais",
                table: "EnviosMensagemMensais");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataGeracao",
                table: "EnviosMensagemMensais",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnviosMensagemMensais",
                table: "EnviosMensagemMensais",
                columns: new[] { "Competencia", "ContatoTelefone", "EstabelecimentoCnpj", "DataGeracao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EnviosMensagemMensais",
                table: "EnviosMensagemMensais");

            migrationBuilder.DropColumn(
                name: "DataGeracao",
                table: "EnviosMensagemMensais");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnviosMensagemMensais",
                table: "EnviosMensagemMensais",
                columns: new[] { "Competencia", "ContatoTelefone", "EstabelecimentoCnpj" });
        }
    }
}
