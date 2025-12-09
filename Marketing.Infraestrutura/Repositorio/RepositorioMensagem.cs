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
            var itens = new List<MensagemItem>();

            if (competencia != null)
            {
                var mensagens = _context.Mensagens
                                        .Include(x => x.EnvioMensagemMensal)
                                        .AsSplitQuery()
                                        .Where(x => x.EnvioMensagemMensal.Competencia == competencia).AsQueryable();
                foreach(var mensagem in mensagens)
                {
                    var mensagemItem = _context.MensagemItems
                                               .Where(x=>x.MensagemId == mensagem.Id)
                                               .OrderByDescending(x=> x.DataEvento).FirstOrDefault();
                    if (mensagemItem != null) itens.Add(mensagemItem);
                }

                var itemsGroup = itens.GroupBy(x=>x.MensagemStatus).Select(x=> new { Status = x.Key, Qtde = x.Count()});
                foreach( var itemGroup in itemsGroup){
                    resumo.Add(
                        new ResumoMensagem()
                        {
                            MensagemStatus = itemGroup.Status,
                            Qtd = itemGroup.Qtde
                        }
                    );
                }
            }
            return resumo;
        }

        // public List<ResumoMensagem> BuscaResumoMensagemPorCompetenciaV2(DateTime? competencia)
        // {
        //     var resumo = new List<ResumoMensagem>();
        //     if (competencia != null)
        //     {
        //         var result = (from M in _context.Mensagens
        //                       join MI in _context.MensagemItems on M.Id equals MI.Id
        //                       join EM in _context.EnviosMensagemMensais on M.MensagemId equals EM.MensagemId 
        //                       where M.Competencia == competencia
        //                       into res 
        //                       from MM in res.OrderByDescending(x=>x.DataEvento).Take(1)
        //                       select new { MM.}
        //                       )
               
        //                         .Select(g => new
        //                         {
        //                             Id = g.Key,
        //                             DataEvento = g.Max(v => v.DataEvento)
        //                         });
                
        //         var soma = from MI in _context.MensagemItems 
        //                    join T in trackList on MI.Id equals T.Id 
        //                    where T.DataEvento == MI.DataEvento
        //                    group MI by MI.MensagemStatus into newG
        //                    select new
        //                      {
        //                          MensagemStatus = newG.Key,
        //                          Total = newG.Count()
        //                      }; 
        //         foreach(var item in soma)
        //         {
        //             resumo.Add(
        //                 new ResumoMensagem()
        //                 {
        //                     MensagemStatus = item.MensagemStatus,
        //                     Qtd = item.Total
        //                 }
        //             );
        //         }
        //     }
        //     return resumo;
        // }
    }
}