using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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

            // var estabelecimentos = await (
            //     from r in _context.Redes
            //     join es in _context.Estabelecimentos on r.Nome equals es.RedeNome
            //     join ex in _context.ExtratosVendas on es.Cnpj equals ex.EstabelecimentoCnpj
            //     where ex.Ano == ano && ex.Mes <= mes && r.Nome == estabelecimento.RedeNome
            //     select es
            //     ).ToListAsync();

            var rede = await _context.Set<Rede>().
                Include(es => es.Estabelecimentos).
                    ThenInclude(es => es.ExtratoVendas.
                                    Where(ex => ex.Competencia > DateTime.Now.AddMonths(-12))).
                Where(r => r.Nome == estabelecimento.RedeNome).
                FirstOrDefaultAsync();

            if (estabelecimento.Cnpj == "20289192000105")
            {
                Console.WriteLine("Erro");
            }

            if (rede == null) return 1;
            var estabelecimentoSort = rede.Estabelecimentos.
                                      OrderByDescending(x => x.IncidenciaMedia).
                                      ToList();
            int posicaoNaRede = 0;
            posicaoNaRede = estabelecimentoSort.FindIndex(x=>x.Cnpj == estabelecimento.Cnpj);
            return posicaoNaRede == -1 ? 1 : ++posicaoNaRede;
        }
    }
}
