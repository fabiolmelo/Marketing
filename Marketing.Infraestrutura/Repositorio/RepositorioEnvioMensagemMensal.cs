using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioEnvioMensagemMensal : Repository<EnvioMensagemMensal>, IRepositorioEnvioMensagemMensal
    {
        private readonly DataContext _context;
        public RepositorioEnvioMensagemMensal(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<List<EnvioMensagemMensal>> BuscarMensagensNaoEnviadas(DateTime competencia)
        {
            return await _context.Set<EnvioMensagemMensal>()
                                 .Where(x => x.Competencia == competencia && x.MensagemId == null)
                                 .ToListAsync();
        }
    }
}