using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Aggregates;
using Play.Persistence.Sql;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.ValueObjects;

namespace Play.Loyalty.Persistence.Sql.Configuration;

// You can configure a navigation in the model to be included every time the entity is loaded from the database using AutoInclude method
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
internal class LoyaltyEntityConfiguration : IEntityTypeConfiguration<LoyaltyProgram>, IEntityTypeConfiguration<LoyaltyMember>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<LoyaltyProgram> builder)
    {
        builder.ToTable($"{nameof(LoyaltyProgram)}s").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.PrivateProperty<LoyaltyProgram, SimpleStringId>($"_MerchantId");
        builder.HasOne<LoyaltyProgram, RewardsProgram, SimpleStringId>($"_RewardsProgram", "RewardsProgramId");
        builder.HasMany<LoyaltyProgram, Discount, SimpleStringId>("_Discounts", "Discount");
    }

    public void Configure(EntityTypeBuilder<LoyaltyMember> builder)
    {
        builder.ToTable($"{nameof(LoyaltyMember)}s").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.PrivateProperty<LoyaltyMember, SimpleStringId>($"_MerchantId");
        builder.HasOne<LoyaltyMember, Rewards, SimpleStringId>("_Rewards", "RewardsId");
        builder.PrivateProperty<LoyaltyMember, RewardsNumber>($"_RewardsNumber");
        builder.PrivateProperty<LoyaltyMember, Phone>($"_Phone");
        builder.PrivateProperty<LoyaltyMember, Email?>($"_Email");
    }

    #endregion
}