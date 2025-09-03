using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoMensagem
    {
        Task InserirMensagem(WhatsAppResponseResult whatsAppResponseResult);
    }
}