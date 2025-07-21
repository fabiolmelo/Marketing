using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdStringAsync(string id);
        Task<PagedResponse<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filtros = null, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<int> Count(Expression<Func<T, bool>> expression);
        Task<T?> FindByPredicate(Expression<Func<T, bool>> expression);
    }
}