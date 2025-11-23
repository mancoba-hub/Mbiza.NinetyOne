using Mbiza.NinetyOne.TopScorers.Application.DTOs;
using Mbiza.NinetyOne.TopScorers.Domain.Entities;

namespace Mbiza.NinetyOne.TopScorers.Application.Interfaces
{
    public interface ITopScorersService
    {
        Task<IEnumerable<DtoTopScorer>> GetTopScorersAsync(CancellationToken cancellationToken = default);

        Task<DtoTopScorer?> GetTopScorerByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<DtoTopScorer>> CreateTopScorersAsync(string data, CancellationToken cancellationToken = default);

        Task<IEnumerable<DtoTopScorer>> CreateTopScorersAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
