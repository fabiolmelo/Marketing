using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoEmail
    {
        Task<bool> EnviarEmailASync(string emailDestinatario);
    }
}