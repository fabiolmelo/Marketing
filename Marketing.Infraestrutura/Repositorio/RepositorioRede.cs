using System.Linq.Expressions;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.PagedResponse;
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
            var rede = _context.Set<Rede>().
                Include(es => es.Estabelecimentos).
                    ThenInclude(es => es.ExtratoVendas.
                                    Where(ex => ex.Competencia > DateTime.Now.AddMonths(-12))).
                Where(r => r.Nome == estabelecimento.RedeNome).
                FirstOrDefault();

            if (rede == null) return 1;
            var estabelecimentoSort = rede.Estabelecimentos.
                                      Where(x=>x.IncidenciaMedia > 0).
                                      OrderByDescending(x => x.IncidenciaMedia).
                                      ToList();
            int posicaoNaRede = 0;
            posicaoNaRede = estabelecimentoSort.FindIndex(x=>x.Cnpj == estabelecimento.Cnpj);
            return posicaoNaRede == -1 ? 1 : ++posicaoNaRede;
        }

        public async Task<PagedResponse<List<Rede>>> GetAllAsync(int pageNumber, int pageSize,
                                                                Expression<Func<Rede, bool>>? filtros = null,
                                                                params Expression<Func<Rede, object>>[] includes)
        {
            var query = _context.Redes.AsNoTracking();
            if (filtros != null)
            {
                query = query.Where(filtros);
            }
            if (includes != null)
            {
                includes.Aggregate(query, (current, includes) => current.Include(includes));
            }
            query = query.OrderBy(x => x.Nome);
            var totalRecords = await query.CountAsync();
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);
            var redes = await query.ToListAsync();
            return new PagedResponse<List<Rede>>(redes, pageNumber, pageSize, totalRecords);
        }
    }
}
