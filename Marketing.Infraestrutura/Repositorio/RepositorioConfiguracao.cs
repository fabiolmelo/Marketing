using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioConfiguracao : Repository<ConfiguracaoApp>, IRepositorioConfiguracao
    {
        private readonly DataContext _dataContext;
        public RepositorioConfiguracao(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public ConfiguracaoApp BuscarConfiguracao()
        {
            return _dataContext.Configuracoes.FirstOrDefault(x=>x.Id == 1) ?? new ConfiguracaoApp();
        }
    }
}