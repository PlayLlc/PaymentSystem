using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.ValueObjects;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.ValueObjects;
using Play.Persistence.Sql;

namespace Play.Loyalty.Persistence.Sql.Configuration;

internal class LoyaltyEntityConfiguration : IEntityTypeConfiguration<Programs>, IEntityTypeConfiguration<Member>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<Programs> builder)
    {
        builder.ToTable($"{nameof(Programs)}").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.PrivateProperty<Programs, SimpleStringId>("_MerchantId").ValueGeneratedOnAdd();
        builder.HasOne<Programs, RewardProgram, SimpleStringId>("_RewardProgram", "RewardsProgramId");
        builder.HasOne<Programs, DiscountProgram, SimpleStringId>("_DiscountProgram", "DiscountProgramId");
    }

    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable($"{nameof(Member)}s").HasKey(x => x.Id);

        // Simple Properties
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.PrivateProperty<Member, SimpleStringId>("_MerchantId").ValueGeneratedOnAdd();
        builder.HasOne<Member, Rewards, SimpleStringId>("_Rewards", "RewardsId");
        builder.PrivateProperty<Member, RewardsNumber>("_RewardsNumber").ValueGeneratedOnAdd();
        builder.PrivateProperty<Member, Phone>("_Phone");
        builder.PrivateProperty<Member, Name>("_Name");
        builder.PrivateProperty<Member, Email?>("_Email");
    }

    #endregion
}