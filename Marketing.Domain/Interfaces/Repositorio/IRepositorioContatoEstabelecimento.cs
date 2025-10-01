using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioContatoEstabelecimento : IRepository<ContatoEstabelecimento>
    {
        void UpdateContatoEstabelecimento(ContatoEstabelecimento contatoEstabelecimento);
    }
}