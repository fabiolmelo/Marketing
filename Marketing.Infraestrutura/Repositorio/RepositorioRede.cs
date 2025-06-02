using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioRede : Repository<Rede>, IRepositorioRede
    {
        private readonly DataContext _context;
        public RepositorioRede(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, 
                                                                    Estabelecimento estabelecimento)
        {
            var ano = competencia.Year;
            var mes = competencia.Month;

            var rede = await _context.Redes.AsNoTracking().
                                Include(x => x.Estabelecimentos.Where(x => x.RedeId == estabelecimento.RedeId)).
                                ThenInclude(x => x.ExtratoVendas.Where(x => x.Ano == ano && x.Mes <= mes).
                                OrderByDescending(x => x.IncidenciaReal)).FirstAsync();
            return rede.Estabelecimentos.ToList().IndexOf(estabelecimento) + 1;
            
        }
    }
}
