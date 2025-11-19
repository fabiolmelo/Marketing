using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.PagedResponse;

namespace Marketing.Application.Servicos
{
    public class ServicoEnvioMensagemMensal : Servico<EnvioMensagemMensal>, IServicoEnvioMensagemMensal
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicoEnvioMensagemMensal(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarMensagensNaoEnviadas(int pageNumber, int pageSize)
        {
            return await _unitOfWork.repositorioEnvioMensagemMensal.BuscarMensagensNaoEnviadas(pageNumber, pageSize);
        }

        public async Task<PagedResponse<List<EnvioMensagemMensal>>> BuscarTodasMensagens(int pageNumber, int pageSize)
        {
            return await _unitOfWork.repositorioEnvioMensagemMensal.BuscarTodasMensagens(pageNumber, pageSize);
        }

        public async Task<List<EnvioMensagemMensal>> BuscarTodasMensagensNaoEnviadas()
        {
            return await _unitOfWork.repositorioEnvioMensagemMensal.BuscarTodasMensagensNaoEnviadas();
        }

        // public async Task<EnvioMensagemMensal?> GetByCompetenciaEstabelecimentoContato(DateTime? competencia, string cnpj, string telefone)
        // {
        //     return await _unitOfWork.repositorioEnvioMensagemMensal.GetByCompetenciaEstabelecimentoContato(competencia, cnpj, telefone);
        // }
    }
}