using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServico<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdStringAsync(string id);
        Task<PagedResponse<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10, Expression<Func<T, bool>>? filtros = null, params Expression<Func<T, object>>[] includes);
        Task<T?> FindByPredicate(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<int> Count(Expression<Func<T, bool>> expression);
        
    }
}