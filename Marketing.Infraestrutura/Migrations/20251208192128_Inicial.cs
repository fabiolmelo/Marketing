using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaApiUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FoneFrom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoteProcessamento = table.Column<int>(type: "int", nullable: true),
                    IntervaloEntreDisparos = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Telefone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AceitaMensagem = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    DataAceite = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    RecusaMensagem = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    DataRecusa = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Email = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<Guid>(type: "char(80)", maxLength: 80, nullable: false, collation: "ascii_general_ci"),
                    UltimaCompetenciaEnviada = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OrigemContato = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Telefone);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImportacaoEfetuada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeArquivoServer = table.Column<string>(type: "VARCHAR(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataImportacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportacaoEfetuada", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mensagens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaMensagemId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagens", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MetaWebhookResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Response = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaWebhookResponses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Redes",
                columns: table => new
                {
                    Nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Logo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redes", x => x.Nome);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TemplateImportarPlanilhas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Template = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateImportarPlanilhas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DadosPlanilha",
                columns: table => new
                {
                    ImportacaoEfetuadaId = table.Column<int>(type: "int", nullable: false),
                    AnoMes = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cnpj = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Uf = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Restaurante = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalPedidos = table.Column<int>(type: "int", nullable: false),
                    PedidosComCocaCola = table.Column<int>(type: "int", nullable: false),
                    IncidenciaReal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Meta = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PrecoUnitarioMedio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalPedidosNaoCapturados = table.Column<int>(type: "int", nullable: false),
                    ReceitaNaoCapturada = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Rede = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Linha = table.Column<int>(type: "int", nullable: true),
                    Planilha = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnviosMensagemMensais",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Competencia = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContatoTelefone = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RedeNome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeFranquia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataGeracao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MensagemId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnviosMensagemMensais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnviosMensagemMensais_Mensagens_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagens",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MensagemItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MensagemId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataEvento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MensagemStatus = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensagemItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensagemItems_Mensagens_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Estabelecimentos",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RedeNome = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazaoSocial = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Endereco = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bairro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Uf = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cep = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UltimoPdfGerado = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estabelecimentos", x => x.Cnpj);
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_Redes_RedeNome",
                        column: x => x.RedeNome,
                        principalTable: "Redes",
                        principalColumn: "Nome");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContatoEstabelecimento",
                columns: table => new
                {
                    ContatoTelefone = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstabelecimentoCnpj = table.Column<string>(type: "varchar(14)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExtratosVendas",
                columns: table => new
                {
                    Competencia = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "varchar(14)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    TotalPedidos = table.Column<int>(type: "int", nullable: false),
                    PedidosComCocaCola = table.Column<int>(type: "int", nullable: false),
                    IncidenciaReal = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Meta = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    PrecoUnitarioMedio = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    TotalPedidosNaoCapturados = table.Column<int>(type: "int", nullable: false),
                    ReceitaNaoCapturada = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    CorVerdeGrafico = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    CorTransparenteGrafico = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    CorVermelhaGrafico = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtratosVendas", x => new { x.Competencia, x.EstabelecimentoCnpj });
                    table.ForeignKey(
                        name: "FK_ExtratosVendas_Estabelecimentos_EstabelecimentoCnpj",
                        column: x => x.EstabelecimentoCnpj,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                column: "MensagemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MENSAGEM",
                table: "EnviosMensagemMensais",
                columns: new[] { "Competencia", "ContatoTelefone", "EstabelecimentoCnpj", "DataGeracao" });

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_RedeNome",
                table: "Estabelecimentos",
                column: "RedeNome");

            migrationBuilder.CreateIndex(
                name: "IX_ExtratosVendas_EstabelecimentoCnpj",
                table: "ExtratosVendas",
                column: "EstabelecimentoCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_MensagemItems_MensagemId",
                table: "MensagemItems",
                column: "MensagemId");

            migrationBuilder.CreateIndex(
                name: "IX_MENSAGEM_METAMENSAGEMID",
                table: "Mensagens",
                column: "MetaMensagemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");

            migrationBuilder.DropTable(
                name: "ContatoEstabelecimento");

            migrationBuilder.DropTable(
                name: "DadosPlanilha");

            migrationBuilder.DropTable(
                name: "EnviosMensagemMensais");

            migrationBuilder.DropTable(
                name: "ExtratosVendas");

            migrationBuilder.DropTable(
                name: "MensagemItems");

            migrationBuilder.DropTable(
                name: "MetaWebhookResponses");

            migrationBuilder.DropTable(
                name: "TemplateImportarPlanilhas");

            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "ImportacaoEfetuada");

            migrationBuilder.DropTable(
                name: "Estabelecimentos");

            migrationBuilder.DropTable(
                name: "Mensagens");

            migrationBuilder.DropTable(
                name: "Redes");
        }
    }
}
