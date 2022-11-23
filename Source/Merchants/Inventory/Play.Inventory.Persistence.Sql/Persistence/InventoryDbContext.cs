using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.Entities;
using Play.Inventory.Contracts.Enums;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.ValueObjects;
using Play.Inventory.Persistence.Sql.Configuration;
using Play.Persistence.Sql;

namespace Play.Inventory.Persistence.Sql.Persistence;

public sealed class InventoryDbContext : DbContext
{
    #region Static Metadata

    public const string _DatabaseName = "Inventory";

    #endregion

    #region Constructor

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    #endregion

    #region Instance Members

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ValueConverters.ConfigureCommonConverters(configurationBuilder);
        DomainValueConverters.ConfigureCommonConverters(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Enums

        InventoryEntityConfiguration inventoryEntityConfiguration = new InventoryEntityConfiguration();

        #region Enums

        builder.Entity<StockAction>().ToTable($"{nameof(StockActions)}").HasKey(a => a.Value);
        builder.Entity<StockAction>().HasData(StockActions.Empty.GetAll().Select(e => new StockAction(e)));

        #endregion

        #region Entities

        builder.Entity<Price>().ToTable($"Prices").HasKey(x => x.Id);
        builder.Entity<Price>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Price>().Property(x => x.NumericCurrencyCode);
        builder.Entity<Price>().Property(x => x.Amount);

        builder.Entity<Alerts>().ToTable($"{nameof(Alerts)}").HasKey(x => x.Id);
        builder.Entity<Alerts>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Alerts>().Property(x => x.IsActive);
        builder.Entity<Alerts>().Property(x => x.LowInventoryThreshold);

        builder.Entity<Store>().ToTable($"{nameof(Store)}s").HasKey(x => x.Id);
        builder.Entity<Store>().Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Entity<Locations>().ToTable($"{nameof(Locations)}").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Locations>().HasKey(x => x.Id);
        builder.Entity<Locations>().Property<bool>("_AllLocations").HasColumnName("AllLocations");
        builder.Entity<Locations>().HasMany<Store>("_Stores");

        builder.Entity<Variation>().ToTable($"{nameof(Variation)}s").HasKey(x => x.Id);
        builder.Entity<Variation>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Variation>().Property("_Sku");
        builder.Entity<Variation>().Property("_Name");
        builder.Entity<Variation>().HasOne("_Price");

        inventoryEntityConfiguration.Configure(builder.Entity<Category>());
        inventoryEntityConfiguration.Configure(builder.Entity<Item>());

        #endregion
    }

    #endregion
}