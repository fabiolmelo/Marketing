using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;
using System.Linq.Expressions;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoContato : IServico<Contato>
    {
        Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        Task<PagedResponse<List<Contato>>> GetAllContatos(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtro);
        Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj);
        Task AtualizarAssociacaoContatoEstabelecimento(List<DadosPlanilha> dadosPlanilhas);
    }
}
