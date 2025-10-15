﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketing.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Telefone = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    AceitaMensagem = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataAceite = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RecusaMensagem = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataRecusa = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Token = table.Column<Guid>(type: "TEXT", maxLength: 50, nullable: false),
                    UltimaCompetenciaEnviada = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Telefone);
                });

            migrationBuilder.CreateTable(
                name: "ImportacaoEfetuada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeArquivoServer = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    DataImportacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportacaoEfetuada", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mensagem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Redes",
                columns: table => new
                {
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Logo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redes", x => x.Nome);
                });

            migrationBuilder.CreateTable(
                name: "DadosPlanilha",
                columns: table => new
                {
                    ImportacaoEfetuadaId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnoMes = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cnpj = table.Column<string>(type: "TEXT", nullable: false),
                    Uf = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    Restaurante = table.Column<string>(type: "TEXT", nullable: false),
                    TotalPedidos = table.Column<int>(type: "INTEGER", nullable: false),
                    PedidosComCocaCola = table.Column<int>(type: "INTEGER", nullable: false),
                    IncidenciaReal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Meta = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrecoUnitarioMedio = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPedidosNaoCapturados = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceitaNaoCapturada = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rede = table.Column<string>(type: "TEXT", nullable: false),
                    Fone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosPlanilha", x => new { x.ImportacaoEfetuadaId, x.Cnpj, x.AnoMes });
                    table.ForeignKey(
                        name: "FK_DadosPlanilha_ImportacaoEfetuada_ImportacaoEfetuadaId",
                        column: x => x.ImportacaoEfetuadaId,
                        principalTable: "ImportacaoEfetuada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnviosMensagemMensais",
                columns: table => new
                {
                    Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "TEXT", nullable: false),
                    ContatoTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    MensagemId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnviosMensagemMensais", x => new { x.Competencia, x.ContatoTelefone, x.EstabelecimentoCnpj });
                    table.ForeignKey(
                        name: "FK_EnviosMensagemMensais_Mensagem_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensagemItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MensagemId = table.Column<string>(type: "TEXT", nullable: false),
                    DataEvento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MensagemStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensagemItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensagemItem_Mensagem_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estabelecimentos",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "TEXT", maxLength: 14, nullable: false),
                    RedeNome = table.Column<string>(type: "TEXT", nullable: true),
                    RazaoSocial = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Uf = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    UltimoPdfGerado = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estabelecimentos", x => x.Cnpj);
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_Redes_RedeNome",
                        column: x => x.RedeNome,
                        principalTable: "Redes",
                        principalColumn: "Nome");
                });

            migrationBuilder.CreateTable(
                name: "ContatoEstabelecimento",
                columns: table => new
                {
                    ContatoTelefone = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatoEstabelecimento", x => new { x.ContatoTelefone, x.EstabelecimentoCnpj });
                    table.ForeignKey(
                        name: "FK_ContatoEstabelecimento_Contatos_ContatoTelefone",
                        column: x => x.ContatoTelefone,
                        principalTable: "Contatos",
                        principalColumn: "Telefone",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContatoEstabelecimento_Estabelecimentos_EstabelecimentoCnpj",
                        column: x => x.EstabelecimentoCnpj,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtratosVendas",
                columns: table => new
                {
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    Mes = table.Column<int>(type: "INTEGER", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "TEXT", nullable: false),
                    Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalPedidos = table.Column<int>(type: "INTEGER", nullable: false),
                    PedidosComCocaCola = table.Column<int>(type: "INTEGER", nullable: false),
                    IncidenciaReal = table.Column<decimal>(type: "NUMERIC", precision: 8, scale: 4, nullable: false),
                    Meta = table.Column<decimal>(type: "NUMERIC", precision: 18, scale: 4, nullable: false),
                    PrecoUnitarioMedio = table.Column<decimal>(type: "NUMERIC", precision: 8, scale: 2, nullable: false),
                    TotalPedidosNaoCapturados = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceitaNaoCapturada = table.Column<decimal>(type: "NUMERIC", precision: 8, scale: 2, nullable: false),
                    CorVerdeGrafico = table.Column<decimal>(type: "NUMERIC", precision: 8, scale: 2, nullable: false),
                    CorTransparenteGrafico = table.Column<decimal>(type: "NUMERIC", precision: 8, scale: 2, nullable: false),
                    CorVermelhaGrafico = table.Column<decimal>(type: "NUMERIC", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtratosVendas", x => new { x.Ano, x.Mes, x.EstabelecimentoCnpj });
                    table.ForeignKey(
                        name: "FK_ExtratosVendas_Estabelecimentos_EstabelecimentoCnpj",
                        column: x => x.EstabelecimentoCnpj,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContatoEstabelecimento_EstabelecimentoCnpj",
                table: "ContatoEstabelecimento",
                column: "EstabelecimentoCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_CONTATO_TOKEN",
                table: "Contatos",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_EnviosMensagemMensais_MensagemId",
                table: "EnviosMensagemMensais",
                column: "MensagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_RedeNome",
                table: "Estabelecimentos",
                column: "RedeNome");

            migrationBuilder.CreateIndex(
                name: "IX_ExtratosVendas_EstabelecimentoCnpj",
                table: "ExtratosVendas",
                column: "EstabelecimentoCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_MensagemItem_MensagemId",
                table: "MensagemItem",
                column: "MensagemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContatoEstabelecimento");

            migrationBuilder.DropTable(
                name: "DadosPlanilha");

            migrationBuilder.DropTable(
                name: "EnviosMensagemMensais");

            migrationBuilder.DropTable(
                name: "ExtratosVendas");

            migrationBuilder.DropTable(
                name: "MensagemItem");

            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "ImportacaoEfetuada");

            migrationBuilder.DropTable(
                name: "Estabelecimentos");

            migrationBuilder.DropTable(
                name: "Mensagem");

            migrationBuilder.DropTable(
                name: "Redes");
        }
    }
}
