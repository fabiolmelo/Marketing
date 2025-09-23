using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoEnvioMensagemMensal : IServico<EnvioMensagemMensal>
    {
         public Task<List<EnvioMensagemMensal>> BuscarMensagensNaoEnviadas(DateTime competencia);
    }
}