using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MerchantPortal.Infrastructure.Persistence.Sql;

internal class MerchantPortalDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MerchantPortalDbContext(DbContextOptions<MerchantPortalDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DbSet<CompanyEntity> Companies { get; init; }
    public DbSet<MerchantEntity> Merchants { get; init; }
    public DbSet<StoreEntity> Stores { get; init; }
    public DbSet<TerminalEntity> Terminals { get; init; }
}
