using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioMensagem : IRepository<Mensagem>
    {
        Task<List<Mensagem>> GetAllMensagemsAsync(DateTime competencia);
    }
}