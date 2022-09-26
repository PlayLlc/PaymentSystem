using Microsoft.EntityFrameworkCore;

using Play.Merchants.Domain.Entities;

namespace Play.Merchants.Persistence.Sql.Sql;

internal class MerchantPortalDbContext : DbContext
{
    #region Instance Values

    public DbSet<Company> Companies { get; init; }
    public DbSet<Merchant> Merchants { get; init; }
    public DbSet<Store> Stores { get; init; }
    public DbSet<Terminal> Terminals { get; init; }

    #endregion

    #region Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MerchantPortalDbContext(DbContextOptions<MerchantPortalDbContext> options) : base(options)
    { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    #endregion
}