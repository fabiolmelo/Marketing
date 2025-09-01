using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoMeta
    {
        Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato, string urlExtrato);
        Task<ServicoExtratoResponseDto> EnviarExtrato(Contato contato, string urlExtrato);
        Task<bool> EnviarTesteASync(Contato contato);
    }
}