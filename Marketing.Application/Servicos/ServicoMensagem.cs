using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.Interfaces.UnitOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoMensagem : Servico<Mensagem>, IServicoMensagem
    {
        public ServicoMensagem(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}