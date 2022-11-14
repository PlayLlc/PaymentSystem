using Microsoft.EntityFrameworkCore;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Persistence.Configuration;

namespace Play.Underwriting.Persistence.Persistence;

public class UnderwritingDbContext : DbContext
{
    public UnderwritingDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        UnderwritingEntitiesConfiguration configuration = new UnderwritingEntitiesConfiguration();

        configuration.Configure(builder.Entity<Individual>());
        configuration.Configure(builder.Entity<Address>());
        configuration.Configure(builder.Entity<Alias>());

        base.OnModelCreating(builder);
    }
}
