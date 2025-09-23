using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoEnvioMensagemMensal : Servico<EnvioMensagemMensal>, IServicoEnvioMensagemMensal
    {
        private readonly IRepositorioEnvioMensagemMensal _repositorioEnvioMensagemMensal;
        public ServicoEnvioMensagemMensal(IRepositorioEnvioMensagemMensal repository) : base(repository)
        {
            _repositorioEnvioMensagemMensal = repository;
        }

        public async Task<List<EnvioMensagemMensal>> BuscarMensagensNaoEnviadas(DateTime competencia)
        {
            return await _repositorioEnvioMensagemMensal.BuscarMensagensNaoEnviadas(competencia);
        }
    }
}