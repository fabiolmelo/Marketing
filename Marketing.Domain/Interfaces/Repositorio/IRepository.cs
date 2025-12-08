using System.Linq.Expressions;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdStringAsync(string id);
        T? GetByIdString(string id);
        Task<T?> GetByIdChaveComposta(string id1, string id2);
        Task AddAsync(T entity);
        void Add(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Delete(T entity);
        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<int> Count(Expression<Func<T, bool>> expression);
        Task<T?> FindByPredicate(Expression<Func<T, bool>> expression);
        Task<List<T>> FindAllByPredicate(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void UpdateCommit(T entity);
        void UpdateRange(List<T> entities);
        Task<List<T>> GetAll();
    }
}