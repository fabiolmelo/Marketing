using System.Linq.Expressions;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dataContext.AddRangeAsync(entities);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await _dataContext.Set<T>().AnyAsync(expression);
        }
        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _dataContext.Set<T>().CountAsync(expression);
        }

        public void Delete(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
        }

        public async Task<T?> FindByPredicate(Expression<Func<T, bool>> expression)
        {
            return await _dataContext.Set<T>().
                                      Where(expression).FirstOrDefaultAsync();
        }
        public async Task<List<T>> FindAllByPredicate(Expression<Func<T, bool>> expression)
        {
            return await _dataContext.Set<T>().
                                      Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdChaveComposta(string id1, string id2)
        {
            return await _dataContext.Set<T>().FindAsync(id1, id2);
        }

        public async Task<T?> GetByIdStringAsync(string id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            //_dataContext.Update(entity);
        }

        public void UpdateRange(List<T> entities)
        {
            _dataContext.UpdateRange(entities);
        }

        public void UpdateCommit(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public async Task<List<T>> GetAll()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        
    }
}