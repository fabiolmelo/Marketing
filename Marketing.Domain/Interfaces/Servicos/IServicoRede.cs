using System.Linq.Expressions;
using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoRede : IServico<Rede>
    {
        Task AtualizarRedesViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento);
        Task<PagedResponse<List<Rede>>> GetAllRedesAsync(int pageNumber, int pageSize, Expression<Func<Rede, bool>>? filtros = null, params Expression<Func<Rede, object>>[] includes);
    }
}