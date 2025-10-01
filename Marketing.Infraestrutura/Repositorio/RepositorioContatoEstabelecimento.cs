using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioContatoEstabelecimento : Repository<ContatoEstabelecimento>, IRepositorioContatoEstabelecimento
    {
        private readonly DataContext _dataContext;
        public RepositorioContatoEstabelecimento(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public void UpdateContatoEstabelecimento(ContatoEstabelecimento contatoEstabelecimento)
        {
            throw new NotImplementedException();
        }
    }
}