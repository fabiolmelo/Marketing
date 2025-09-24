using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnityOfWork;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoExtratoVenda : Servico<ExtratoVendas>, IServicoExtratoVendas
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicoExtratoVenda(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork  = unitOfWork;
        }
        public async Task<DateTime> BuscarCompetenciaVigente()
        {
            return await _unitOfWork.repositorioExtratoVendas.BuscarCompetenciaVigente();
        }
    }
}