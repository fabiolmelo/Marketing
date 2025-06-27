using Marketing.Domain.Entidades;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoMeta
    {
        Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato);
        //Task<bool> EnviarFechamentoMensalASync(Contato contato, ArquivoMensal arquivo);
        Task<bool> EnviarTesteASync(Contato contato);
    }
}