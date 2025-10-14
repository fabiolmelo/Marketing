using System.Linq.Expressions;
using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioRede : IRepository<Rede>
    {
        Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento EstabelecimentoId);
        Task<PagedResponse<List<Rede>>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<Rede, bool>>? filtros = null, params Expression<Func<Rede, object>>[] includes);
    }
}
