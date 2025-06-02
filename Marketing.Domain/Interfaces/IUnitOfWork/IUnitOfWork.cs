using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork 
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task<int> CommitAsync();
    }
}