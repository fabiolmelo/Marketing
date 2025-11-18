using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.PagedResponse;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioEnvioMensagemMensal : Repository<EnvioMensagemMensal>, IRepositorioEnvioMensagemMensal
    {
        private readonly DataContext _dataContext;
        public RepositorioEnvioMensagemMensal(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarMensagensNaoEnviadas(int pageNumber, int pageSize)
        {
            var query = _dataContext.Set<EnvioMensagemMensal>()
                                    .Include(x=>x.Mensagem)
                                    .Where(x => x.MensagemId == null);                  
            
            var totalRecords = await query.CountAsync();
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PagedResponse<List<EnvioMensagemMensal>>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }

        public async Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarTodasMensagens(int pageNumber, int pageSize) 
        {
            var query = _dataContext.Set<EnvioMensagemMensal>()
                                    .Include(x => x.Mensagem).AsQueryable();
            var totalRecords = await query.CountAsync();
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PagedResponse<List<EnvioMensagemMensal>>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }

        public async Task<List<EnvioMensagemMensal>> BuscarTodasMensagensNaoEnviadas(DateTime? competencia = null)
        {
            IQueryable<EnvioMensagemMensal> query = _dataContext.Set<EnvioMensagemMensal>().Where(x => x.MensagemId == null);
            if(competencia != null)
            {
                query = query.Where(x => x.Competencia == competencia);
            }
            return await query.ToListAsync();
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<EnvioMensagemMensal?> GetByCompetenciaEstabelecimentoContato(DateTime? competencia, string cnpj, string telefone)
        {
            return await _dataContext.Set<EnvioMensagemMensal>()
                                     .Include(x=>x.Mensagem)
                                        .ThenInclude(c=>c.MensagemItems)
                                     .Where(y=>y.Mensagem.MensagemItems.Any(x=>x.MensagemStatus == MensagemStatus.Falha))
                                     .OrderByDescending(x=>x.DataGeracao)
                                     .FirstOrDefaultAsync(x => x.Competencia == competencia &&
                                                               x.EstabelecimentoCnpj == cnpj &&
                                                               x.ContatoTelefone == telefone);
        }
    }
}