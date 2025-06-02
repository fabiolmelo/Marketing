using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoContato : IServico<Contato>
    {
        Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
    }
}
