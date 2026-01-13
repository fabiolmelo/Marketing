using System;
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

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    AceitaMensagem = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    DataAceite = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    RecusaMensagem = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    DataRecusa = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    Token = table.Column<Guid>(type: "TEXT", maxLength: 80, nullable: false),
                    UltimaCompetenciaEnviada = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrigemCadastro = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Telefone);
                });

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuncoesClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncoesClaims", x => x.Id);
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
                name: "Mensagens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    MetaMensagemId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaWebhookResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Response = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaWebhookResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Redes",
                columns: table => new
                {
                    Nome = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Logo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redes", x => x.Nome);
                });

            migrationBuilder.CreateTable(
                name: "TemplateImportarPlanilhas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Template = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateImportarPlanilhas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Id = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosFuncoes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosFuncoes", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "UsuariosLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "UsuariosTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosTokens", x => new { x.UserId, x.LoginProvider, x.Name });
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
                    Fone = table.Column<string>(type: "TEXT", nullable: false),
                    Linha = table.Column<int>(type: "INTEGER", nullable: true),
                    Planilha = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "TEXT", nullable: false),
                    ContatoTelefone = table.Column<string>(type: "TEXT", nullable: false),
                    RedeNome = table.Column<string>(type: "TEXT", nullable: false),
                    NomeFranquia = table.Column<string>(type: "TEXT", nullable: false),
                    DataGeracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MensagemId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnviosMensagemMensais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnviosMensagemMensais_Mensagens_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MensagemItems",
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
                    table.PrimaryKey("PK_MensagemItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensagemItems_Mensagens_MensagemId",
                        column: x => x.MensagemId,
                        principalTable: "Mensagens",
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
                    Endereco = table.Column<string>(type: "TEXT", nullable: true),
                    Numero = table.Column<string>(type: "TEXT", nullable: true),
                    Complemento = table.Column<string>(type: "TEXT", nullable: true),
                    Bairro = table.Column<string>(type: "TEXT", nullable: true),
                    Cidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Uf = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
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
                    ContatoTelefone = table.Column<string>(type: "TEXT", nullable: false),
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
                    Competencia = table.Column<DateTime>(type: "DATE", nullable: false),
                    EstabelecimentoCnpj = table.Column<string>(type: "TEXT", nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    Mes = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPedidos = table.Column<int>(type: "INTEGER", nullable: false),
                    PedidosComCocaCola = table.Column<int>(type: "INTEGER", nullable: false),
                    IncidenciaReal = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Meta = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    PrecoUnitarioMedio = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    TotalPedidosNaoCapturados = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "IX_Funcoes_NormalizedName",
                table: "Funcoes",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MensagemItems_MensagemId",
                table: "MensagemItems",
                column: "MensagemId");

            migrationBuilder.CreateIndex(
                name: "IX_MENSAGEM_METAMENSAGEMID",
                table: "Mensagens",
                column: "MetaMensagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "NormalizedEmail");
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
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "FuncoesClaims");

            migrationBuilder.DropTable(
                name: "MensagemItems");

            migrationBuilder.DropTable(
                name: "MetaWebhookResponses");

            migrationBuilder.DropTable(
                name: "TemplateImportarPlanilhas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "UsuariosClaims");

            migrationBuilder.DropTable(
                name: "UsuariosFuncoes");

            migrationBuilder.DropTable(
                name: "UsuariosLogins");

            migrationBuilder.DropTable(
                name: "UsuariosTokens");

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
