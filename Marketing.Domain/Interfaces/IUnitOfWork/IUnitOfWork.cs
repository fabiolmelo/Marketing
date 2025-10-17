using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Domain.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        IRepository<T> GetRepository<T>() where T : class;
        public IRepositorioContato repositorioContato { get; }
        public IRepositorioEnvioMensagemMensal repositorioEnvioMensagemMensal { get; }
        public IRepositorioEstabelecimento repositorioEstabelecimento { get; }
        public IRepositorioExtratoVendas repositorioExtratoVendas { get; }
        public IRepositorioProcessamentoMensal repositorioProcessamentoMensal { get; }
        public IRepositorioRede repositorioRede { get; }
        public IRepositorioMensagem repositorioMensagem { get; }
        
    }
}