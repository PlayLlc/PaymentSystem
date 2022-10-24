using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.ValueObjects;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Globalization.Time;

namespace Play.Accounts.Persistence.Sql.Configuration;

internal class AccountsEntityConfiguration : IEntityTypeConfiguration<UserRegistration>, IEntityTypeConfiguration<MerchantRegistration>,
    IEntityTypeConfiguration<Merchant>, IEntityTypeConfiguration<UserIdentity>
{
    #region Static Metadata

    private static readonly ValueConverter<DateTimeUtc, DateTime> _DateTimeUtcConverter = new(x => x, y => new DateTimeUtc(y));

    #endregion

    #region Instance Members

    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.ToTable($"{nameof(User)}s");
        builder.HasKey(x => x.Id);

        builder.Property<string>(x => x.MerchantId).HasColumnName("MerchantId");
        builder.Property<string>(x => x.TerminalId).HasColumnName("TerminalId");
        builder.Property<bool>(x => x.IsActive).HasColumnName("IsActive");

        builder.HasOne<Password>(nameof(Password));
        builder.HasOne<Address>(nameof(Address));
        builder.HasOne<PersonalDetail>(nameof(PersonalDetail));
        builder.HasOne<Contact>(nameof(Contact));
    }

    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable($"{nameof(Merchant)}s");
        builder.HasKey("_Id");
        builder.Property<Name>("_CompanyName").HasColumnName("CompanyName").HasConversion<string>(x => x, y => new Name(y));
        builder.Property<bool>("_IsActive").HasColumnName("IsActive");

        builder.HasOne<Address>("_Address");
        builder.HasOne<BusinessInfo>("_BusinessInfo");
    }

    public void Configure(EntityTypeBuilder<MerchantRegistration> builder)
    {
        builder.ToTable($"{nameof(MerchantRegistration)}s");
        builder.HasKey("_Id");
        builder.Property<DateTimeUtc>("_RegistrationDate").HasColumnName("RegistrationDate").HasConversion<DateTime>(_DateTimeUtcConverter);
        builder.Property<Name?>("_CompanyName").HasColumnName("CompanyName").HasConversion<string>(x => x ?? string.Empty, y => new Name(y));
        builder.Property<MerchantRegistrationStatus>("_Status").HasColumnName("Status").HasConversion<string>(x => x, y => new MerchantRegistrationStatus(y));

        builder.HasOne<Address>("_Address");
        builder.HasOne<BusinessInfo>("_BusinessInfo");
    }

    public void Configure(EntityTypeBuilder<UserRegistration> builder)
    {
        builder.ToTable($"{nameof(UserRegistration)}s");
        builder.HasKey("_Id");
        builder.Property<DateTimeUtc>("_RegistrationDate").HasColumnName("RegistrationDate").HasConversion<DateTime>(_DateTimeUtcConverter);
        builder.Property<string>("_Username").HasColumnName("Username");
        builder.Property<string>("_HashedPassword").HasColumnName("HashedPassword");
        builder.Property<bool>("_HasEmailBeenVerified").HasColumnName("HasEmailBeenVerified");
        builder.Property<bool>("_HasPhoneBeenVerified").HasColumnName("HasPhoneBeenVerified");
        builder.Property<UserRegistrationStatus>("_Status").HasColumnName("Status").HasConversion<string>(x => x, y => new UserRegistrationStatus(y));

        builder.HasOne<Address>("_Address");
        builder.HasOne<Contact>("_Contact");
        builder.HasOne<PersonalDetail>("_PersonalDetail");
        builder.HasOne<ConfirmationCode>("_EmailConfirmation");
        builder.HasOne<ConfirmationCode>("_SmsConfirmation");
    }

    #endregion
}