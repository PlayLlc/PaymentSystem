using Microsoft.EntityFrameworkCore;

using Play.MerchantPortal.Domain.Entities;

namespace Play.MerchantPortal.Infrastructure.Persistence.Sql;

internal class MerchantPortalDbContext : DbContext
{
    #region Instance Values

    public DbSet<CompanyEntity> Companies { get; init; }
    public DbSet<MerchantEntity> Merchants { get; init; }
    public DbSet<StoreEntity> Stores { get; init; }
    public DbSet<TerminalEntity> Terminals { get; init; }

    #endregion

    #region Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MerchantPortalDbContext(DbContextOptions<MerchantPortalDbContext> options) : base(options)
    { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    #endregion
}