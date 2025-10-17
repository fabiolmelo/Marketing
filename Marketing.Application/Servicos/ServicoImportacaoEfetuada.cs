using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoImportacaoEfetuada : Servico<ImportacaoEfetuada>
    {
        public ServicoImportacaoEfetuada(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}