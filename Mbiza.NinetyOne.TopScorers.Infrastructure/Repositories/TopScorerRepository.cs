using Mbiza.NinetyOne.TopScorers.Application.Interfaces;
using Mbiza.NinetyOne.TopScorers.Domain.Entities;

namespace Mbiza.NinetyOne.TopScorers.Infrastructure.Repositories
{
    public class TopScorerRepository : GenericRepository<TopScorer>, ITopScorerRepository
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TopScorerRepository class using the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used by the repository. Cannot be null.</param>
        public TopScorerRepository(MbizaDbContext context) : base(context)
        {

        }

        #endregion

        #region Implementation of ITopScorerRepository

        /// <summary>
        /// Asynchronously adds a collection of top scorer records to the data store.
        /// </summary>
        /// <param name="topScorers">The list of top scorer entities to add. Cannot be null or contain null elements.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of the added top scorer
        /// entities.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public async Task<List<TopScorer>> AddTopScorersAsync(List<TopScorer> topScorers, CancellationToken cancellationToken = default)
        {
            var addedTopScorers = await AddRangeAsync(topScorers, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return addedTopScorers;
        }

        /// <summary>
        /// Asynchronously retrieves the top scorer whose name matches the specified value.
        /// </summary>
        /// <param name="name">The name of the scorer to search for. The search is case-insensitive.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the top scorer with the
        /// specified name, or <see langword="null"/> if no matching scorer is found.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public async Task<TopScorer?> GetTopScorerByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await GetByConditionAsync(x => x.FirstName.ToLower() == name.ToLower() || x.SecondName.ToLower() == name.ToLower(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves a list of top scorers.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of top scorers. The list
        /// will be empty if no scorers are found.</returns>
        /// <exception cref="NotImplementedException">The method is not implemented.</exception>
        public async Task<List<TopScorer>> GetTopScorersAsync(CancellationToken cancellationToken = default)
        {
            return await GetAllAsync(cancellationToken);
        }

        #endregion
    }
}
