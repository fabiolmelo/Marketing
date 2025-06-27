using Marketing.Domain.Entidades;
using System;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioRede : IRepository<Rede>
    {
        Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento EstabelecimentoId);
    }
}
