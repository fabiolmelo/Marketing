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
            var rede = await _context.Set<Rede>().AsNoTracking().
                Include(es => es.Estabelecimentos).
                    ThenInclude(es => es.ExtratoVendas.
                                    Where(ex => ex.Competencia > DateTime.Now.AddMonths(-12))).
                Where(r => r.Nome == estabelecimento.RedeNome).
                FirstOrDefaultAsync();

            if (rede == null) return 1;
            var estabelecimentoSort = rede.Estabelecimentos.
                                      Where(x=>x.IncidenciaMedia > 0).
                                      OrderByDescending(x => x.IncidenciaMedia).
                                      ToList();
            int posicaoNaRede = 0;
            posicaoNaRede = estabelecimentoSort.FindIndex(x=>x.Cnpj == estabelecimento.Cnpj);
            return posicaoNaRede == -1 ? 1 : ++posicaoNaRede;
        }
    }
}
