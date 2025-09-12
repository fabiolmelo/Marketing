using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioMensagem : Repository<Mensagem>, IRepositorioMensagem 
    {
        public RepositorioMensagem(DataContext dataContext) : base(dataContext)
        {
        }
    }
}