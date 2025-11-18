using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioMensagem : IRepository<Mensagem>
    {
        Task<Mensagem?> FindByIdIncludeEventosAsync(string id);
        Task<List<Mensagem>> GetAllMensagemsAsync(DateTime? competencia);
        List<ResumoMensagem> BuscaResumoMensagemPorCompetencia(DateTime? competencia);
        //List<ResumoMensagem> BuscaResumoMensagemPorCompetenciaV2(DateTime? competencia);

    }
}