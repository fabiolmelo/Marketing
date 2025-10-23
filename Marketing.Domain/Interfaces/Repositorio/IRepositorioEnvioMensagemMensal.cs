using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioEnvioMensagemMensal : IRepository<EnvioMensagemMensal>
    {
        Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarMensagensNaoEnviadas(int pageNumber, int pageSize);
        Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarTodasMensagens(int pageNumber, int pageSize); 
        Task<List<EnvioMensagemMensal>> BuscarTodasMensagensNaoEnviadas(DateTime? competencia = null); 
        Task<EnvioMensagemMensal?> GetByIdChaveComposta3(DateTime id1, string id2, string id3);
    }
}