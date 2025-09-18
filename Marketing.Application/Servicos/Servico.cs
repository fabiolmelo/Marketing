using System.Linq.Expressions;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.PagedResponse;
using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Application.Servicos
{
    public class Servico<T> : IServico<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Servico(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task AddAsyncWithCommit(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await _repository.Any(expression);
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _repository.Count(expression);
        }

        public void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public async Task<T?> FindByPredicate(Expression<Func<T, bool>> expression)
        {
            return await _repository.FindByPredicate(expression);
        }

        public async Task<PagedResponse<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10,
                                Expression<Func<T, bool>>? filtros = null,
                                params Expression<Func<T, object>>[] includes)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, filtros, includes);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T?> GetByIdStringAsync(string id)
        {
            return await _repository.GetByIdStringAsync(id);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }

        public void UpdateWithCommit(T entity)
        {
            _repository.Update(entity);
        }
    }
}