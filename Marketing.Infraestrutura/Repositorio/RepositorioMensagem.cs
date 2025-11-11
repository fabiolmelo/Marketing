using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioMensagem : Repository<Mensagem>, IRepositorioMensagem
    {
        private readonly DataContext _context;
        
        public RepositorioMensagem(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<Mensagem?> FindByIdIncludeEventosAsync(string id)
        {
            return await _context.Mensagens
                                 .Include(x => x.MensagemItems)
                                 .AsSplitQuery()
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Mensagem>> GetAllMensagemsAsync(DateTime? competencia)
        {
            var mensagens = new List<Mensagem>();
            if (competencia != null)
            {
                var envios = await _context.EnviosMensagemMensais.Where(x => x.Competencia == competencia)
                                     .Include(x => x.Mensagem)
                                     .ToListAsync();
                foreach (var envio in envios)
                {
                    var mensagem = await _context.Mensagens
                                            .Include(x => x.EnvioMensagemMensal) 
                                            .Include(x => x.MensagemItems)
                                        .FirstOrDefaultAsync(x => x.Id == envio.MensagemId);
                    if (mensagem != null) mensagens.Add(mensagem);
                }
            }
            return mensagens;
        }

        public List<ResumoMensagem> BuscaResumoMensagemPorCompetencia(DateTime? competencia)
        {
            var resumo = new List<ResumoMensagem>();
            if (competencia != null)
            {
                var trackList = _context.MensagemItems.Include(x => x.Mensagem)
                                                      .Where(x => x.Mensagem.EnvioMensagemMensal != null &&
                                                                  x.Mensagem.EnvioMensagemMensal.Competencia == competencia)
                                .GroupBy(v => v.Id)
                                .Select(g => new
                                {
                                    Id = g.Key,
                                    DataEvento = g.Max(v => v.DataEvento)
                                });
                
                var soma = from MI in _context.MensagemItems 
                           join T in trackList on MI.Id equals T.Id 
                           where T.DataEvento == MI.DataEvento
                           group MI by MI.MensagemStatus into newG
                           select new
                             {
                                 MensagemStatus = newG.Key,
                                 Total = newG.Count()
                             }; 
                foreach(var item in soma)
                {
                    resumo.Add(
                        new ResumoMensagem()
                        {
                            MensagemStatus = item.MensagemStatus,
                            Qtd = item.Total
                        }
                    );
                }
            }
            return resumo;
        }
       
    }
}