using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.HighPerformance.Enumerables;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Persistence.Sql.Configuration;
using Play.Persistence.Sql;

namespace Play.Loyalty.Persistence.Sql.Persistence;

public sealed class LoyaltyDbContext : DbContext
{
    #region Static Metadata

    public const string DatabaseName = "Loyalty";

    #endregion

    #region Constructor

    public LoyaltyDbContext(DbContextOptions<LoyaltyDbContext> options) : base(options)
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

        LoyaltyEntityConfiguration loyaltyEntityConfiguration = new LoyaltyEntityConfiguration();

        #region Entities

        builder.Entity<Discount>().ToTable($"{nameof(Discount)}s").HasKey(a => a.Id);
        builder.Entity<Discount>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Discount>().PrivateProperty<Discount, SimpleStringId>("_ItemId");
        builder.Entity<Discount>().PrivateProperty<Discount, SimpleStringId>("_VariationId");
        builder.Entity<Discount>().PrivateProperty<Discount, NumericCurrencyCode>("_NumericCurrencyCode");
        builder.Entity<Discount>().PrivateProperty<Discount, ulong>("_Price");

        builder.Entity<Rewards>().ToTable($"{nameof(Rewards)}").HasKey(x => x.Id);
        builder.Entity<Rewards>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Rewards>().PrivateProperty<Rewards, uint>("_Points");
        builder.Entity<Rewards>().Property(x => EF.Property<MoneyValueObject>(x, "_Balance").Amount).HasColumnName("Amount");
        builder.Entity<Rewards>().Property(x => EF.Property<MoneyValueObject>(x, "_Balance").NumericCurrencyCode).HasColumnName("NumericCurrencyCode");

        builder.Entity<RewardsProgram>().ToTable($"{nameof(RewardsProgram)}").HasKey(x => x.Id);
        builder.Entity<RewardsProgram>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<RewardsProgram>().PrivateProperty<RewardsProgram, bool>("_IsActive");
        builder.Entity<RewardsProgram>().PrivateProperty<RewardsProgram, uint>("_PointsRequired");
        builder.Entity<RewardsProgram>().PrivateProperty<RewardsProgram, uint>("_PointsPerDollar");
        builder.Entity<RewardsProgram>().Property(x => EF.Property<MoneyValueObject>(x, "_RewardAmount").Amount).HasColumnName("Amount");
        builder.Entity<RewardsProgram>()
            .Property(x => EF.Property<MoneyValueObject>(x, "_RewardAmount").NumericCurrencyCode)
            .HasColumnName("NumericCurrencyCode");

        loyaltyEntityConfiguration.Configure(builder.Entity<LoyaltyProgram>());
        loyaltyEntityConfiguration.Configure(builder.Entity<LoyaltyMember>());

        #endregion
    }

    #endregion
}