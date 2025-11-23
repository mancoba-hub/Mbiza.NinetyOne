using Mbiza.NinetyOne.TopScorers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mbiza.NinetyOne.TopScorers.Infrastructure
{
    public class MbizaDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MbizaDbContext class using the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext. Must not be null.</param>
        public MbizaDbContext(DbContextOptions<MbizaDbContext> options) : base(options)
        {
        }

        #endregion

        #region Properties

        public DbSet<TopScorer> TopScorers { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Configures the model for the context by using the specified model builder.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context. Provides a fluent API for configuring entity types,
        /// relationships, and database mappings.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
