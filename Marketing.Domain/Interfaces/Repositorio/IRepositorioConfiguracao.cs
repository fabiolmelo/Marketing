using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioConfiguracao : IRepository<ConfiguracaoApp>
    {
        ConfiguracaoApp BuscarConfiguracao();
    }
}