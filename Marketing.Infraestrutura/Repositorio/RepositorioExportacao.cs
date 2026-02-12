using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioExportacao : IRepositorioExportacao
    {
        private readonly DataContext _dataContext;

        public RepositorioExportacao(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<List<ExportacaoV1>> BuscarRelatorioV1(string rede)
        {
            var relatorio = new List<ExportacaoV1>();
            using var connection = _dataContext.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT EX.Competencia AnoMes, E.UF, E.CIDADE, E.CNPJ, E.RazaoSocial Restaurante,EX.TotalPedidos, EX.PedidosComCocaCola, EX.IncidenciaReal, EX.Meta, EX.PrecoUnitarioMedio, EX.TotalPedidosNaoCapturados, EX.ReceitaNaoCapturada, coalesce(CC.ContatoTelefone, '') Telefone FROM ExtratosVendas EX JOIN Estabelecimentos E ON E.Cnpj == EX.EstabelecimentoCnpj AND E.RedeNome == EX.EstabelecimentoRedeNome LEFT JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY EstabelecimentoCnpj) AS [ORD],C.EstabelecimentoCnpj,C.ContatoTelefone, C.EstabelecimentoRedeNome FROM ContatoEstabelecimento C ) CC ON CC.EstabelecimentoCnpj = E.CNPJ AND CC.EstabelecimentoRedeNome = E.RedeNome AND CC.[ORD] = 1 WHERE EX.Competencia >= '2025-01-01' AND E.RedeNome = @rede ORDER BY EX.Competencia, E.CNPJ";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@rede";
            parameter.Value = rede;
            command.Parameters.Add(parameter);
            
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                relatorio.Add(
                    new ExportacaoV1(
                        reader.GetDateTime(0),reader.GetString(1),reader.GetString(2),reader.GetString(3),
                        reader.GetString(4),reader.GetInt32(5),reader.GetInt32(6),reader.GetDecimal(7),
                        reader.GetDecimal(8),reader.GetDecimal(9),reader.GetInt32(10),reader.GetDecimal(11),
                        reader.GetString(12)
                    )
                );
            }
            return relatorio;
        }
    }
}