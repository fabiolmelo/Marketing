using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioEnvioMensagemMensal : IRepository<EnvioMensagemMensal>
    {
        Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarMensagensNaoEnviadas(int pageNumber, int pageSize);
        Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarTodasMensagens(int pageNumber, int pageSize); 
        Task<List<EnvioMensagemMensal>> BuscarTodasMensagensNaoEnviadas(DateTime? competencia = null);
        // Task<EnvioMensagemMensal?> GetByCompetenciaEstabelecimentoContato(DateTime? competencia, string cnpj, string telefone);
        Task CommitAsync();
        void Commit();
    }
}