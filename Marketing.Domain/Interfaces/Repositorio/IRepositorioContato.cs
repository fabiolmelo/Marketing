using System.Linq.Expressions;
using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioContato : IRepository<Contato>
    {
        Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj);
        Task<Contato?> BuscarContatosIncludeEstabelecimento(string telefone);
        Task<PagedResponse<List<Contato>>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null, params Expression<Func<Contato, object>>[] includes);
    }
}