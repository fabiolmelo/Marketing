using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioEnvioMensagemMensal : IRepository<EnvioMensagemMensal>
    {
        Task<List<EnvioMensagemMensal>> BuscarMensagensNaoEnviadas(DateTime competencia);
    }
}