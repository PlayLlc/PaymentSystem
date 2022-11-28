using Microsoft.EntityFrameworkCore;

using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Persistence.Sql.Configuration;

namespace Play.Underwriting.Persistence.Sql.Persistence;

public class UnderwritingDbContext : DbContext
{
    #region Constructor

    public UnderwritingDbContext(DbContextOptions options) : base(options)
    { }

    #endregion

    #region Instance Members

    protected override void OnModelCreating(ModelBuilder builder)
    {
        UnderwritingEntitiesConfiguration configuration = new UnderwritingEntitiesConfiguration();

        configuration.Configure(builder.Entity<Individual>());
        configuration.Configure(builder.Entity<Address>());
        configuration.Configure(builder.Entity<Alias>());

        base.OnModelCreating(builder);
    }

    #endregion
}