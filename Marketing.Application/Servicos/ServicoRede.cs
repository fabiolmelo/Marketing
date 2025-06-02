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
            var redes = await _unitOfWork.GetRepository<Rede>().GetAllAsync(1,999999);
            var redesCadastradas = redes.Dados.Select(x=>x.Nome).Distinct().ToArray();
            var redesNaoCadastradas = dadosPlanilhas.
                                      Where(x =>!redesCadastradas.Contains(x.Rede) && !x.Rede.IsNullOrEmpty()).
                                      Select(x=>x.Rede).Distinct().
                                      ToList();
            
            foreach(string redeString in redesNaoCadastradas){
                var rede = new Rede(redeString);
                await _unitOfWork.GetRepository<Rede>().AddAsync(rede);
            } 
            await _unitOfWork.CommitAsync();
        }

        public async Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento)
        {
            return await _repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
        }
    }
}