using Mbiza.NinetyOne.TopScorers.Application.Commom;
using Mbiza.NinetyOne.TopScorers.Application.DTOs;
using Mbiza.NinetyOne.TopScorers.Application.Interfaces;
using Mbiza.NinetyOne.TopScorers.Domain.Entities;
using System.Text;

namespace Mbiza.NinetyOne.TopScorers.Services
{
    public class TopScorersService : ITopScorersService
    {
        #region Properties

        private readonly ITopScorerRepository _topScorerRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TopScorersService class using the specified top scorer repository.
        /// </summary>
        /// <param name="topScorerRepository">The repository used to access and manage top scorer data. Cannot be null.</param>
        public TopScorersService(ITopScorerRepository topScorerRepository)
        {
            _topScorerRepository = topScorerRepository;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Asynchronously retrieves a collection of top scorer data transfer objects filtered by the specified name.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of top
        /// scorer data transfer objects matching the specified name. If no matches are found, the collection will be
        /// empty.</returns>
        public async Task<IEnumerable<DtoTopScorer>> GetTopScorersAsync(CancellationToken cancellationToken = default)
        {
            List<TopScorer> topScorers = await _topScorerRepository.GetTopScorersAsync(cancellationToken);
            var topScorer = topScorers.Where(x => x.Score == topScorers.Max(s => s.Score)).ToList();
            var stdout = GetTopScorer(topScorer);
            Console.WriteLine(stdout);
            return topScorer.ToDtoTopScorers();
        }
        
        /// <summary>
        /// Asynchronously retrieves the top scorer whose name matches the specified value.
        /// </summary>
        /// <param name="name">The name of the scorer to search for. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="TopScorer"/>
        /// instance matching the specified name, or <c>null</c> if no such scorer is found.</returns>
        /// <exception cref="NotImplementedException">Thrown to indicate that the method is not yet implemented.</exception>
        public async Task<DtoTopScorer?> GetTopScorerByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            TopScorer? topScorer = await _topScorerRepository.GetTopScorerByNameAsync(name, cancellationToken);
            if (topScorer == null)
                return null;

            return topScorer.ToDtoTopScorer();
        }

        /// <summary>
        /// Asynchronously retrieves the top scorers from the specified data source.
        /// </summary>
        /// <param name="data">A string containing the data source or input from which to determine the top scorers. Cannot be null or
        /// empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string with information about
        /// the top scorers.</returns>
        /// <exception cref="NotImplementedException">Thrown in all cases as the method is not yet implemented.</exception>
        public async Task<IEnumerable<DtoTopScorer>> CreateTopScorersAsync(string data, CancellationToken cancellationToken = default)
        {
            int count = -1;
            List<TopScorer> topScorers = [];
            string[] delimiters = ["\r\n", "\\r\\n"];
            string[] splitData = data.Split(delimiters, StringSplitOptions.None);
            foreach (var item in splitData)
            {
                count++;
                if (count == 0)
                    continue;

                string[] record = item.Split(',');
                if (record.Length != 3)
                    continue;

                TopScorer topScorer = new()
                {
                    FirstName = record[0].Trim(),
                    SecondName = record[1].Trim(),
                    Score = DataTypeExtension.ToInt32(record[2])
                };
                topScorers.Add(topScorer);
            }
            topScorers = await _topScorerRepository.AddTopScorersAsync(topScorers);
            return topScorers.ToDtoTopScorers();
        }

        /// <summary>
        /// Asynchronously retrieves a JSON-formatted string containing the top scorers from the provided data stream.
        /// </summary>
        /// <param name="stream">The input stream containing the data to be processed. The stream must be readable and positioned at the
        /// start of the data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a JSON string listing the top
        /// scorers extracted from the stream.</returns>
        /// <exception cref="NotImplementedException">Thrown in all cases as the method is not yet implemented.</exception>
        public async Task<IEnumerable<DtoTopScorer>> CreateTopScorersAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            int count = 0;
            List<TopScorer> topScorers = [];
            using (StreamReader sr = new(stream))
            {
                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    if (count == 0)
                    {
                        count++;
                        continue;
                    }

                    if (line == null)
                        continue;

                    string[] record = line.Split(',');
                    if (record.Length != 3)
                        continue;

                    TopScorer topScorer = new()
                    {
                        FirstName = record[0],
                        SecondName = record[1],
                        Score = DataTypeExtension.ToInt32(record[2])
                    };                    
                    topScorers.Add(topScorer);
                    
                }
            }
            topScorers = await _topScorerRepository.AddTopScorersAsync(topScorers);
            return topScorers.ToDtoTopScorers();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the top scorer
        /// </summary>
        /// <param name="topScorers"></param>
        /// <returns></returns>
        private string GetTopScorer(List<TopScorer> topScorers)
        {           
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("=============================================================");
            foreach (var item in topScorers.OrderBy(s => s.FirstName))
            {
                stringBuilder.AppendLine($"== {item.FirstName.ToString().TrimStart().TrimEnd()} {item.SecondName.ToString().TrimStart().TrimEnd()}");
            }
            stringBuilder.AppendLine($"== Score: {topScorers.First().Score}");
            stringBuilder.AppendLine("=============================================================");
            return stringBuilder.ToString();
        }

        #endregion
    }
}
