using System.Linq.Expressions;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.PagedResponse;
using Marketing.Domain.Interfaces.IUnityOfWork;

namespace Marketing.Application.Servicos
{
    public class Servico<T> : IServico<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public Servico(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(T entity)
        {
            await _unitOfWork.GetRepository<T>().AddAsync(entity);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await _unitOfWork.GetRepository<T>().Any(expression);
        }

        public async Task<int> CommitAsync()
        {
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _unitOfWork.GetRepository<T>().Count(expression);
        }

        public void Delete(T entity)
        {
            _unitOfWork.GetRepository<T>().Delete(entity);
        }

        public async Task<T?> FindByPredicate(Expression<Func<T, bool>> expression)
        {
            return await _unitOfWork.GetRepository<T>().FindByPredicate(expression);
        }

        public async Task<PagedResponse<List<T>>> GetAllAsync(int pageNumber = 1, int pageSize = 10,
                                Expression<Func<T, bool>>? filtros = null,
                                params Expression<Func<T, object>>[] includes)
        {
            return await _unitOfWork.GetRepository<T>().GetAllAsync(pageNumber, pageSize, filtros, includes);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<T>().GetByIdAsync(id);
        }

        public async Task<T?> GetByIdStringAsync(string id)
        {
            return await _unitOfWork.GetRepository<T>().GetByIdStringAsync(id);
        }

        public void Update(T entity)
        {
            _unitOfWork.GetRepository<T>().Update(entity);
        }
    }
}