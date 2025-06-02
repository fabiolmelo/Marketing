using Marketing.Application.Servicos;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.UnitOfWork;
using Microsoft.IdentityModel.Tokens;

namespace Marketing.Domain.Interfaces.Servicos
{
    public class ServicoRede : Servico<Rede>, IServicoRede
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositorioRede _repositorioRede;
        public ServicoRede(IUnitOfWork unitOfWork, IRepositorioRede repositorioRede) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repositorioRede = repositorioRede;
        }

        public async Task AtualizarRedesViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                var rede = await _unitOfWork.GetRepository<Rede>().GetByIdStringAsync(linhaPlanilha.Rede);
                if (rede == null)
                {
                    rede = new Rede(linhaPlanilha.Rede);
                    await _unitOfWork.GetRepository<Rede>().AddAsync(rede);
                    await _unitOfWork.CommitAsync();
                }
            }
        }

        public async Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento)
        {
            return await _repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
        }
    }
}