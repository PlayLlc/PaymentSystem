using MerchantPortal.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MerchantPortal.Infrastructure.Persistence;

internal class MerchantPortalDbContext : DbContext
{
    public MerchantPortalDbContext(DbContextOptions<MerchantPortalDbContext> options) : base(options) { }

    public DbSet<CompanyEntity> Companies { get; init; }
    public DbSet<MerchantEntity> Merchants { get; init; }
    public DbSet<StoreEntity> Stores { get; init; }
    public DbSet<TerminalEntity> Terminals { get; init; }

}
