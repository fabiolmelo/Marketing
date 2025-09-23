using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioRede : IRepository<Rede>
    {
        Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento EstabelecimentoId);
    }
}
