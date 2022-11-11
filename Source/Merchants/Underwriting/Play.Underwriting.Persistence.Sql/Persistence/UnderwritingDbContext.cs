using Microsoft.EntityFrameworkCore;
using Play.Merchants.Underwriting.Persistence.Configuration;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Entities;

namespace Play.Merchants.Underwriting.Persistence.Persistence;

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
