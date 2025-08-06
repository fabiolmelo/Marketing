using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoContato : IServico<Contato>
    {
        Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        Task<PagedResponse<Contato>> GetAllContatos(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtro);
    }
}
