using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketing.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Telefone = table.Column<string>(type: "TEXT", nullable: false),
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
                    NomeArquivoServer = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DataImportacao = table.Column<DateTime>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportacaoEfetuada", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Redes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redes", x => x.Id);
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
                name: "Estabelecimentos",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "TEXT", maxLength: 14, nullable: false),
                    RedeId = table.Column<int>(type: "INTEGER", nullable: true),
                    RazaoSocial = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Uf = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estabelecimentos", x => x.Cnpj);
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_Redes_RedeId",
                        column: x => x.RedeId,
                        principalTable: "Redes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstabelecimentoContato",
                columns: table => new
                {
                    ContatosTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    EstabelecimentosCnpj = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstabelecimentoContato", x => new { x.ContatosTelefone, x.EstabelecimentosCnpj });
                    table.ForeignKey(
                        name: "FK_EstabelecimentoContato_Contatos_ContatosTelefone",
                        column: x => x.ContatosTelefone,
                        principalTable: "Contatos",
                        principalColumn: "Telefone",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstabelecimentoContato_Estabelecimentos_EstabelecimentosCnpj",
                        column: x => x.EstabelecimentosCnpj,
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
                    IncidenciaReal = table.Column<decimal>(type: "TEXT", precision: 2, nullable: false),
                    Meta = table.Column<decimal>(type: "TEXT", precision: 2, nullable: false),
                    PrecoUnitarioMedio = table.Column<decimal>(type: "TEXT", precision: 2, nullable: false),
                    TotalPedidosNaoCapturados = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceitaNaoCapturada = table.Column<decimal>(type: "TEXT", precision: 2, nullable: false)
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
                name: "IX_CONTATO_TOKEN",
                table: "Contatos",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_EstabelecimentoContato_EstabelecimentosCnpj",
                table: "EstabelecimentoContato",
                column: "EstabelecimentosCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_RedeId",
                table: "Estabelecimentos",
                column: "RedeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtratosVendas_EstabelecimentoCnpj",
                table: "ExtratosVendas",
                column: "EstabelecimentoCnpj");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosPlanilha");

            migrationBuilder.DropTable(
                name: "EstabelecimentoContato");

            migrationBuilder.DropTable(
                name: "ExtratosVendas");

            migrationBuilder.DropTable(
                name: "ImportacaoEfetuada");

            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "Estabelecimentos");

            migrationBuilder.DropTable(
                name: "Redes");
        }
    }
}
