using System.Linq.Expressions;
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

        public async Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarMensagensNaoEnviadas(
                                                                            int pageNumber, int pageSize,
                                                                            bool somenteEnvioPendente)
        {
            var query = _dataContext.EnviosMensagemMensais.AsNoTracking();
            if (somenteEnvioPendente) query.Where(x => x.MensagemId == null);
            var totalRecords = await query.CountAsync();
            query = query.Skip(pageNumber - 1).Take(pageSize);
            return new PagedResponse<List<EnvioMensagemMensal>>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }

        public async Task<List<EnvioMensagemMensal>> BuscarTodasMensagensNaoEnviadas()
        {
            var mensagens = await _dataContext.EnviosMensagemMensais
                                              .Where(x => x.MensagemId == null)
                                              .ToListAsync();
            return mensagens;
        }

        public async Task<EnvioMensagemMensal?> GetByIdChaveComposta3(DateTime id1, string id2, string id3)
        {
            return await _dataContext.EnviosMensagemMensais.FindAsync(id1, id2, id3);
        }
    }
}