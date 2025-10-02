using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoEnvioMensagemMensal : IServico<EnvioMensagemMensal>
    {
        public Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarMensagensNaoEnviadas(int pageNumber,
                                    int pageSize, bool somenteEnvioPendente);
        public Task<List<EnvioMensagemMensal>> BuscarTodasMensagensNaoEnviadas();
        Task<EnvioMensagemMensal?> GetByIdChaveComposta3(DateTime id1, string id2, string id3);  
                                      
    }
}