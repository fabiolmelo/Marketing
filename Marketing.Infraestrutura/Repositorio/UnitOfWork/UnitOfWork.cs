using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;

namespace Marketing.Infraestrutura.Repositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepositorioContato _repositorioContato;
        private IRepositorioEnvioMensagemMensal _repositorioEnvioMensagemMensal;
        private IRepositorioEstabelecimento _repositorioEstabelecimento;
        private IRepositorioExtratoVendas _repositorioExtratoVendas;
        private IRepositorioProcessamentoMensal _repositorioProcessamentoMensal;
        private IRepositorioRede _repositorioRede;
        private IRepositorioMensagem _repositorioMensagem;
        private readonly DataContext _context;
        private Dictionary<Type, object> _repositories;

        public IRepositorioContato repositorioContato => _repositorioContato;
        public IRepositorioEnvioMensagemMensal repositorioEnvioMensagemMensal => _repositorioEnvioMensagemMensal;
        public IRepositorioEstabelecimento repositorioEstabelecimento => _repositorioEstabelecimento;
        public IRepositorioExtratoVendas repositorioExtratoVendas => _repositorioExtratoVendas;
        public IRepositorioProcessamentoMensal repositorioProcessamentoMensal => _repositorioProcessamentoMensal;
        public IRepositorioRede repositorioRede => _repositorioRede;
        public IRepositorioMensagem repositorioMensagem => _repositorioMensagem;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _repositorioContato = new RepositorioContato(context);
            _repositorioEnvioMensagemMensal = new RepositorioEnvioMensagemMensal(context);
            _repositorioEstabelecimento = new RepositorioEstabelecimento(context);
            _repositorioExtratoVendas = new RepositorioExtratoVendas(context);
            _repositorioProcessamentoMensal = new RepositorioFechamentoMensal(context);
            _repositorioRede = new RepositorioRede(context);
            _repositorioMensagem = new RepositorioMensagem(context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> CommitAsync()
        {
            var registrosAlterados = await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
            return registrosAlterados;
        }

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)_repositories[typeof(T)];
            }
            var repository = new Repository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }
    }
}