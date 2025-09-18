using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoExtratoVenda : Servico<ExtratoVendas>, IServicoExtratoVendas
    {
        private readonly IRepositorioExtratoVendas _repositorioExtratoVenda;
        private readonly IRepositorioEstabelecimento _repositorioEstabelecimento;
        public ServicoExtratoVenda(IRepositorioExtratoVendas repository,
                                   IRepositorioEstabelecimento repositorioEstabelecimento) : base(repository)
        {
            _repositorioExtratoVenda = repository;
            _repositorioEstabelecimento = repositorioEstabelecimento;
        }

        public async Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linha in dadosPlanilhas)
            {
                var estabelecimento = await _repositorioEstabelecimento.FindEstabelecimentoIncludeContatoRede(linha.Cnpj);
                var extrato = await _repositorioExtratoVenda.FindByPredicate(
                                                                                x => x.Ano == linha.AnoMes.Year &&
                                                                                x.Mes == linha.AnoMes.Month &&
                                                                                x.EstabelecimentoCnpj == linha.Cnpj);
                if (estabelecimento != null && extrato == null)
                {
                    estabelecimento.ExtratoVendas.Add(
                        new ExtratoVendas(
                            linha.AnoMes.Year,
                            linha.AnoMes.Month,
                            linha.AnoMes,
                            linha.TotalPedidos,
                            linha.PedidosComCocaCola,
                            linha.IncidenciaReal,
                            linha.Meta,
                            linha.PrecoUnitarioMedio,
                            linha.TotalPedidosNaoCapturados,
                            linha.ReceitaNaoCapturada,
                            linha.Cnpj)
                        );
                    _repositorioEstabelecimento.Update(estabelecimento);
                }
            }
        }

        public async Task<DateTime> BuscarCompetenciaVigente()
        {
            return await _repositorioExtratoVenda.BuscarCompetenciaVigente();
        }
    }
}