using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.ValueObjects;
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

        // Complex Type
        builder.RegisterMoneyValueObjectType();

        builder.Entity<Discount>().ToTable($"{nameof(Discount)}s").HasKey(a => a.Id);
        builder.Entity<Discount>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Discount>().PrivateProperty<Discount, SimpleStringId>("_ItemId").ValueGeneratedOnAdd();
        builder.Entity<Discount>().PrivateProperty<Discount, SimpleStringId>("_VariationId").ValueGeneratedOnAdd();
        builder.Entity<Discount>().MoneyValueObjectProperty("_Price");

        builder.Entity<Rewards>().ToTable($"{nameof(Rewards)}").HasKey(x => x.Id);
        builder.Entity<Rewards>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Rewards>().PrivateProperty<Rewards, uint>("_Points");
        builder.Entity<Rewards>().MoneyValueObjectProperty("_Balance");

        builder.Entity<RewardProgram>().ToTable($"{nameof(RewardProgram)}").HasKey(x => x.Id);
        builder.Entity<RewardProgram>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<RewardProgram>().PrivateProperty<RewardProgram, bool>("_IsActive");
        builder.Entity<RewardProgram>().PrivateProperty<RewardProgram, uint>("_PointsRequired");
        builder.Entity<RewardProgram>().PrivateProperty<RewardProgram, uint>("_PointsPerDollar");
        builder.Entity<RewardProgram>().MoneyValueObjectProperty("_RewardAmount");

        builder.Entity<DiscountProgram>().ToTable($"{nameof(DiscountProgram)}s").HasKey(a => a.Id);
        builder.Entity<DiscountProgram>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<DiscountProgram>().PrivateProperty<DiscountProgram, bool>("_IsActive");
        builder.Entity<DiscountProgram>().HasMany<DiscountProgram, Discount, SimpleStringId>("_Discounts", "DiscountId");

        loyaltyEntityConfiguration.Configure(builder.Entity<Member>());
        loyaltyEntityConfiguration.Configure(builder.Entity<Programs>());

        #endregion
    }

    #endregion
}