using Ordering.Domain.Common;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : EntityBase
	{
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T?> GetByIdAsync(int id);

		Task<T> AddAsync(T entity);

		Task UpdateAsync(T entity);
        
		Task DeleteAsync(T entity);
	}
}