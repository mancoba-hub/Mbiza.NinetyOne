using Mbiza.NinetyOne.TopScorers.Application.Interfaces;
using Mbiza.NinetyOne.TopScorers.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mbiza.NinetyOne.TopScorers.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Properties

        protected readonly MbizaDbContext _context;
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the GenericRepository class using the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used for data operations. Cannot be null.</param>
        public GenericRepository(MbizaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        #endregion

        #region Implementation of IGenericRepository<T>

        /// <summary>
        /// Asynchronously retrieves an entity with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified
        /// identifier, or <see langword="null"/> if no entity is found.</returns>
        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves the first entity that matches the specified predicate, or null if no such entity is
        /// found.
        /// </summary>
        /// <param name="predicate">An expression that defines the conditions to filter the entities. Only entities that satisfy this predicate
        /// will be considered.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches
        /// the predicate, or null if no entity is found.</returns>
        public virtual async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves all entities of type T from the data source.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of all entities of type
        /// T.</returns>
        public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the context.
        /// </summary>
        /// <remarks>The entity will be tracked by the context after the operation completes. Changes will
        /// not be persisted to the database until SaveChangesAsync is called.</remarks>
        /// <param name="entity">The entity to add to the context. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous add operation. The task result contains the entity that was added to
        /// the context.</returns>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <summary>
        /// Asynchronously adds the specified entities to the context and marks them for insertion into the database.
        /// </summary>
        /// <remarks>The entities are added to the context and will be inserted into the database when
        /// SaveChangesAsync is called. This method does not save changes to the database.</remarks>
        /// <param name="entities">The list of entities to add. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of entities that were
        /// added to the context.</returns>
        public virtual async Task<List<T>> AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            return entities;

        }

        /// <summary>
        /// Asynchronously marks the specified entity as modified in the context, so that its changes will be saved to
        /// the database on the next save operation.
        /// </summary>
        /// <remarks>This method does not immediately persist changes to the database. To save changes,
        /// call the appropriate save method on the context after calling this method.</remarks>
        /// <param name="entity">The entity to update. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Marks the specified entity as deleted asynchronously by setting its deletion flags.
        /// </summary>
        /// <remarks>This method performs a soft delete by updating the entity's deletion status rather
        /// than removing it from the data store. The entity's IsDeleted property is set to <see langword="true"/>, and
        /// the DeletedAt property is set to the current UTC time.</remarks>
        /// <param name="entity">The entity to mark as deleted. Must not be null and should support soft-delete properties.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            // Soft delete
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Asynchronously saves all changes made in the context to the underlying database.
        /// </summary>
        /// <remarks>This method allows for non-blocking database updates and can be awaited. If the
        /// cancellation token is triggered before the operation completes, the task will be canceled.</remarks>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous save operation.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries
        /// written to the database.</returns>
        public virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }   

        #endregion
    }
}
