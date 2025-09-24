using Marketing.Application.Servicos;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnityOfWork;
using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Domain.Interfaces.Servicos
{
    public class ServicoRede : Servico<Rede>, IServicoRede
    {
        private readonly IUnitOfWork _unityOfWork;

        public ServicoRede(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unityOfWork = unitOfWork;
        }

        public async Task AtualizarRedesViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                var rede = await _unityOfWork.repositorioRede.GetByIdStringAsync(linhaPlanilha.Rede);
                if (rede == null)
                {
                    rede = new Rede(linhaPlanilha.Rede);
                    await _unityOfWork.repositorioRede.AddAsync(rede);
                }
            }
        }

        public async Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento)
        {
            return await _unityOfWork.repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
        }
    }
}