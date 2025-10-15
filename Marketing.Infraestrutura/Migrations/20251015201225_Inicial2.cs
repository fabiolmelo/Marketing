using System;
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
            migrationBuilder.DropForeignKey(
                name: "FK_EnviosMensagemMensais_Mensagem_MensagemId",
                table: "EnviosMensagemMensais");

            migrationBuilder.DropForeignKey(
                name: "FK_MensagemItem_Mensagem_MensagemId",
                table: "MensagemItem");

            migrationBuilder.DropIndex(
                name: "IX_EnviosMensagemMensais_MensagemId",
                table: "EnviosMensagemMensais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MensagemItem",
                table: "MensagemItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem");

            migrationBuilder.RenameTable(
                name: "MensagemItem",
                newName: "MensagemItems");

            migrationBuilder.RenameTable(
                name: "Mensagem",
                newName: "Mensagens");

            migrationBuilder.RenameIndex(
                name: "IX_MensagemItem_MensagemId",
                table: "MensagemItems",
                newName: "IX_MensagemItems_MensagemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Redes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "MensagemId",
                table: "EnviosMensagemMensais",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MensagemItems",
                table: "MensagemItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagens",
                table: "Mensagens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EnviosMensagemMensais_MensagemId",
                table: "EnviosMensagemMensais",
                column: "MensagemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EnviosMensagemMensais_Mensagens_MensagemId",
                table: "EnviosMensagemMensais",
                column: "MensagemId",
                principalTable: "Mensagens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MensagemItems_Mensagens_MensagemId",
                table: "MensagemItems",
                column: "MensagemId",
                principalTable: "Mensagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnviosMensagemMensais_Mensagens_MensagemId",
                table: "EnviosMensagemMensais");

            migrationBuilder.DropForeignKey(
                name: "FK_MensagemItems_Mensagens_MensagemId",
                table: "MensagemItems");

            migrationBuilder.DropIndex(
                name: "IX_EnviosMensagemMensais_MensagemId",
                table: "EnviosMensagemMensais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagens",
                table: "Mensagens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MensagemItems",
                table: "MensagemItems");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Redes");

            migrationBuilder.RenameTable(
                name: "Mensagens",
                newName: "Mensagem");

            migrationBuilder.RenameTable(
                name: "MensagemItems",
                newName: "MensagemItem");

            migrationBuilder.RenameIndex(
                name: "IX_MensagemItems_MensagemId",
                table: "MensagemItem",
                newName: "IX_MensagemItem_MensagemId");

            migrationBuilder.AlterColumn<string>(
                name: "MensagemId",
                table: "EnviosMensagemMensais",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MensagemItem",
                table: "MensagemItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EnviosMensagemMensais_MensagemId",
                table: "EnviosMensagemMensais",
                column: "MensagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnviosMensagemMensais_Mensagem_MensagemId",
                table: "EnviosMensagemMensais",
                column: "MensagemId",
                principalTable: "Mensagem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MensagemItem_Mensagem_MensagemId",
                table: "MensagemItem",
                column: "MensagemId",
                principalTable: "Mensagem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
