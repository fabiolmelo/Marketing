using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioEstabelecimento : IRepository<Estabelecimento>
    {
        Task<PagedResponse<List<Estabelecimento>>> GetAllEstabelecimentos(int? pageNumber, int? pageSize, string? filtro);
        Task<Estabelecimento?> FindEstabelecimentoPorCnpj(string cnpj, string nomeRede);
        Task<Estabelecimento?> FindEstabelecimentoPorCnpjParaPdf(string cnpj, DateTime competencia, string nomeRede);
        Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj, string nomeRede);
        Task<List<Estabelecimento>> GetAllEstabelecimentoPorContato(string telefone);
        Task<List<Estabelecimento>> GetAllEstabelecimentoPorContatoQuePossuiCompetenciaVigente(string telefone);
        Task CommitAsync();
        void Commit();
    }
}
