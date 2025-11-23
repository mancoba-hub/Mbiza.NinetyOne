using Mbiza.NinetyOne.TopScorers.Domain.Common;

namespace Mbiza.NinetyOne.TopScorers.Domain.Entities
{
    public class TopScorer : BaseEntity
    {
        public string FirstName { get; set; } = null!;

        public string SecondName { get; set; } = null!;

        public long Score { get; set; } = 0;
    }
}
