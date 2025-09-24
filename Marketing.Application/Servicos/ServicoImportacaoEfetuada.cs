using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnityOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoImportacaoEfetuada : Servico<ImportacaoEfetuada>
    {
        public ServicoImportacaoEfetuada(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}