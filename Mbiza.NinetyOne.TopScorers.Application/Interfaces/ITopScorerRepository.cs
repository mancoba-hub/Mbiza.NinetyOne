using Mbiza.NinetyOne.TopScorers.Domain.Entities;

namespace Mbiza.NinetyOne.TopScorers.Application.Interfaces
{
    public interface ITopScorerRepository : IGenericRepository<TopScorer>
    {
        Task<TopScorer?> GetTopScorerByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<List<TopScorer>> AddTopScorersAsync(List<TopScorer> topScorers, CancellationToken cancellationToken = default);

        Task<List<TopScorer>> GetTopScorersAsync(CancellationToken cancellationToken = default);
    }
}
