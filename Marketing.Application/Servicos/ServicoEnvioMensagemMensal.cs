using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnityOfWork;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoEnvioMensagemMensal : Servico<EnvioMensagemMensal>, IServicoEnvioMensagemMensal
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicoEnvioMensagemMensal(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EnvioMensagemMensal>> BuscarMensagensNaoEnviadas(DateTime competencia)
        {
            return await _unitOfWork.repositorioEnvioMensagemMensal.BuscarMensagensNaoEnviadas(competencia);
        }
    }
}