using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoMeta
    {
        Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato);
        //Task<bool> EnviarFechamentoMensalASync(Contato contato, ArquivoMensal arquivo);
        Task<bool> EnviarTesteASync(Contato contato);
    }
}