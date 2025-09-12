using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private Dictionary<Type, object> _repositories;
        public UnitOfWork(DataContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }
            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var registros = await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();
                return registros;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ocorreu um erro ao salvar as alterações no banco de dados.", ex);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}