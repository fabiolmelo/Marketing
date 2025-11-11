using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioFechamentoMensal : Repository<EnvioMensagemMensal>, IRepositorioProcessamentoMensal
    {
        private readonly DataContext _context;
        
        public RepositorioFechamentoMensal(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<List<Estabelecimento>> GetAllEstabelecimentosParaGerarPdf(DateTime competencia)
        {
            var dozeMesesAnteriores = competencia.AddYears(-1);
            var estabelecimentos = await _context.Estabelecimentos
                                            .Include(x=>x.Rede)
                                            .Include(x=>x.ContatoEstabelecimentos)
                                                .ThenInclude(x=>x.Contato)
                                            .Include(x => x.ExtratoVendas.Where(e => e.Competencia > dozeMesesAnteriores &&
                                                                                e.Competencia <= competencia))
                                            .Where(x=>x.ExtratoVendas.Count > 0 &&
                                                 x.ExtratoVendas.Any(S=>S.Competencia == competencia))
                                            .AsSplitQuery()
                                            .ToListAsync();
            return estabelecimentos;
        }
    }
}
