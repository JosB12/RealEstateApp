
using System.Linq.Expressions;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity, int id);
        Task DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetAllAsQueryable();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
