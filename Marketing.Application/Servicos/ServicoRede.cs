using System.Linq.Expressions;
using Marketing.Application.Servicos;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnityOfWork;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Servicos
{
    public class ServicoRede : Servico<Rede>, IServicoRede
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicoRede(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AtualizarRedesViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                var rede = await _unitOfWork.repositorioRede.GetByIdStringAsync(linhaPlanilha.Rede);
                if (rede == null)
                {
                    rede = new Rede(linhaPlanilha.Rede);
                    await _unitOfWork.repositorioRede.AddAsync(rede);
                }
            }
        }

        public async Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento)
        {
            return await _unitOfWork.repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
        }

        public async Task<PagedResponse<List<Rede>>> GetAllRedesAsync(int pageNumber, int pageSize,
                                                                 Expression<Func<Rede, bool>>? filtros = null,
                                                                 params Expression<Func<Rede, object>>[] includes)
        {
            return await _unitOfWork.repositorioRede.GetAllAsync(pageNumber, pageSize, filtros);
        }
    }
}