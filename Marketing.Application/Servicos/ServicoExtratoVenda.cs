using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.Interfaces.UnitOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoExtratoVenda : Servico<ExtratoVendas>, IServicoExtratoVendas
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorioEstabelecimento _repositorioEstabelecimento;

        public ServicoExtratoVenda(IUnitOfWork unitOfWork,
                            IRepositorioEstabelecimento repositorioEstabelecimento) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repositorioEstabelecimento = repositorioEstabelecimento;
        }

        public async Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linha in dadosPlanilhas)
            {
                var estabelecimento = await _repositorioEstabelecimento.
                                            FindEstabelecimentoIncludeContatoRede(linha.Cnpj);
                var extrato = await _unitOfWork.GetRepository<ExtratoVendas>().FindByPredicate(
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
                    _unitOfWork.GetRepository<Estabelecimento>().Update(estabelecimento);
                    await _unitOfWork.CommitAsync();
                }
            }
        }
    }
}