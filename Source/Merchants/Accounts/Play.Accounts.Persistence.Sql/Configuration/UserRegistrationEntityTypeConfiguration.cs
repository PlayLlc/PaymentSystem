using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Globalization.Time;

namespace Play.Accounts.Sql.Configurations;

internal class UserRegistrationEntityTypeConfiguration : IEntityTypeConfiguration<UserRegistration>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<UserRegistration> builder)
    {
        builder.ToTable($"{nameof(UserRegistration)}s");
        builder.HasKey(x => x.GetId());
        builder.Property<DateTime>("_RegistrationDate").HasColumnName("RegistrationDate");
        builder.Property<string>("_Username").HasColumnName("Username");
        builder.Property<string>("_HashedPassword").HasColumnName("HashedPassword");

        // password

        builder.OwnsOne<ConfirmationCode>("_EmailConfirmation", b =>
        {
            b.WithOwner().HasPrincipalKey(x => x.GetId());
            b.ToTable($"{nameof(ConfirmationCode)}s");
        });

        builder.OwnsOne<ConfirmationCode>("_SmsConfirmation", b =>
        {
            b.WithOwner().HasPrincipalKey(x => x.GetId());
            b.ToTable($"{nameof(ConfirmationCode)}s");
        });

        builder.OwnsOne<Address>("_Address", b =>
        {
            b.WithOwner().HasPrincipalKey(x => x.GetId());
            b.ToTable($"{nameof(Address)}es");
        });

        builder.OwnsOne<Contact>("_Contact", b =>
        {
            b.WithOwner().HasPrincipalKey(b => b.GetId());
            b.ToTable($"{nameof(Contact)}s");
        });

        builder.OwnsOne<PersonalDetail>("_PersonalDetail", b =>
        {
            b.WithOwner().HasPrincipalKey(b => b.GetId());
            b.ToTable($"{nameof(PersonalDetail)}s");
        });

        builder.OwnsOne<RegistrationStatus>("_Status", b =>
        {
            b.Property(x => x.Value).HasColumnName(nameof(RegistrationStatus));
        });
    }

    #endregion
}