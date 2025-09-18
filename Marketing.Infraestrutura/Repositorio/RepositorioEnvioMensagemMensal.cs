using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioEnvioMensagemMensal : Repository<EnvioMensagemMensal>, IRepositorioEnvioMensagemMensal
    {
        public RepositorioEnvioMensagemMensal(DataContext dataContext) : base(dataContext)
        {
        }

        public Task<List<EnvioMensagemMensal>> BuscarMensagensNaoEnviadas(DateTime competencia)
        {
            throw new NotImplementedException();
        }
    }
}