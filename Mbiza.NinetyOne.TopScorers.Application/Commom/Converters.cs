using Mbiza.NinetyOne.TopScorers.Application.DTOs;
using Mbiza.NinetyOne.TopScorers.Domain.Entities;

namespace Mbiza.NinetyOne.TopScorers.Application.Commom
{
    public static class Converters
    {
        /// <summary>
        /// Converts a collection of domain top scorer entities to a collection of data transfer objects (DTOs)
        /// representing top scorers.
        /// </summary>
        /// <param name="topScorers">The collection of domain top scorer entities to convert. If null or empty, an empty collection is returned.</param>
        /// <returns>An enumerable collection of DtoTopScorer objects corresponding to the input entities. Returns an empty
        /// collection if the input is null or contains no elements.</returns>
        public static IEnumerable<DtoTopScorer> ToDtoTopScorers(this IEnumerable<TopScorer> topScorers)
        {
            if (topScorers == null || !topScorers.Any())
            {
                return [];
            }

            return [.. topScorers.Select(ts => new DtoTopScorer
            {
                FirstName = ts.FirstName,
                SecondName = ts.SecondName,
                Score = ts.Score
            })];
        }

        /// <summary>
        /// Converts a <see cref="TopScorer"/> instance to its corresponding <see cref="DtoTopScorer"/> data transfer
        /// object.
        /// </summary>
        /// <param name="topScorer">The <see cref="TopScorer"/> instance to convert. Can be <see langword="null"/>.</param>
        /// <returns>A <see cref="DtoTopScorer"/> representing the specified top scorer, or <see langword="null"/> if <paramref
        /// name="topScorer"/> is <see langword="null"/>.</returns>
        public static DtoTopScorer ToDtoTopScorer(this TopScorer topScorer)
        {
            return new DtoTopScorer
            {
                FirstName = topScorer.FirstName,
                SecondName = topScorer.SecondName,
                Score = topScorer.Score
            };
        }
    }
}
