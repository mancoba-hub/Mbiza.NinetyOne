using Mbiza.NinetyOne.TopScorers.Domain.Common;
using System.Linq.Expressions;

namespace Mbiza.NinetyOne.TopScorers.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task<List<T>> AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
