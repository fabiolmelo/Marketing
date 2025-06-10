using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.PagedResponse;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioFechamentoMensal : Repository<FechamentoMensal>, IRepositorioProcessamentoMensal
    {
        private readonly DataContext _context;
        
        public RepositorioFechamentoMensal(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<List<Estabelecimento>> GetAllEstabelecimentosParaGerarPdf(DateTime competencia)
        {
            var dozeMesesAnteriores = competencia.AddYears(-1);
            var estabelecimentos = await _context.Estabelecimentos.
                                        AsNoTracking().
                                        Include(x=>x.Rede).
                                        Include(x=>x.Contatos).
                                        Include(x => x.ExtratoVendas.Where(e => e.Competencia > dozeMesesAnteriores)).
                                        Where(x=>x.ExtratoVendas.Count > 0).
                                        ToListAsync();
            var extratos = _context.Set<ExtratoVendas>().ToList();
            return estabelecimentos;
        }
    }
}
