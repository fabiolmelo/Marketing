using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioContato : IRepository<Contato>
    {
        Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj);

        Task<Contato> BuscarContatosIncludeEstabelecimento(string telefone);
    }
}