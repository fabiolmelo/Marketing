using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoEnvioMensagemMensal : Servico<EnvioMensagemMensal>, IServicoEnvioMensagemMensal
    {
        public ServicoEnvioMensagemMensal(IRepository<EnvioMensagemMensal> repository) : base(repository)
        {
        }
    }
}