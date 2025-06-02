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

            var rede = await (from r in _context.Redes
                       join es in _context.Estabelecimentos on r.Nome equals es.RedeNome
                       join ex in _context.ExtratosVendas on es.Cnpj equals ex.EstabelecimentoCnpj
                       where ex.Ano == ano && ex.Mes <= mes && r.Nome == estabelecimento.RedeNome
                       orderby es.IncidenciaMedia descending
                       select  r).Take(1).FirstOrDefaultAsync();


            if (rede == null) return 1;
            List<Estabelecimento> listaEstabelecimentos = rede.Estabelecimentos.ToList();
            if (listaEstabelecimentos == null) return 1;
            int ? posicaoNaRede = listaEstabelecimentos.IndexOf(estabelecimento) + 1;
            return posicaoNaRede ?? 1;
        }
    }
}
