using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoMeta
    {
        Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato, string urlExtrato);
        Task<ServicoExtratoResponseDto> EnviarExtrato(Contato contato, Estabelecimento estabelecimento, string caminhoApp);
        Task<ServicoExtratoResponseDto> EnviarExtratoV2(string idMensagem);
    }
}