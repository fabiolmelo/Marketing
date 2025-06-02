using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioEstabelecimento : IRepository<Estabelecimento>
    {
        Task<PagedResponse<Estabelecimento>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro);

        Task<Estabelecimento?> FindEstabelecimentoPorCnpj(string cnpj);
        Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj);
    }
}
