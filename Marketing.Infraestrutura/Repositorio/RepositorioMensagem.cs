using System.Security.Cryptography.X509Certificates;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioMensagem : Repository<Mensagem>, IRepositorioMensagem
    {
        private readonly DataContext _dataContext;
        
        public RepositorioMensagem(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Mensagem>> GetAllMensagemsAsync(DateTime competencia)
        {
            var mensagens = new List<Mensagem>();
            var envios = await _dataContext.EnviosMensagemMensais.Where(x => x.Competencia == competencia)
                                     .Include(x => x.Mensagem)
                                     .ToListAsync();
            foreach (var envio in envios)
            {
                var mensagem = await _dataContext.Mensagens
                                        .Include(x => x.MensagemItems)
                                    .FirstOrDefaultAsync(x => x.Id == envio.MensagemId);
                if (mensagem != null) mensagens.Add(mensagem);
            }
            return mensagens;
        }
    }
}