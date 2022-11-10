using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.ValueObjects;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Globalization.Time;
using Play.Persistence.Sql;

namespace Play.Accounts.Persistence.Sql.Configuration;

// You can configure a navigation in the model to be included every time the entity is loaded from the database using AutoInclude method
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
internal class AccountsEntityConfiguration : IEntityTypeConfiguration<UserRegistration>, IEntityTypeConfiguration<MerchantRegistration>,
    IEntityTypeConfiguration<Merchant>, IEntityTypeConfiguration<UserIdentity>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.ToTable($"{nameof(User)}s");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MerchantId).HasColumnName("MerchantId");
        builder.Property(x => x.TerminalId).HasColumnName("TerminalId");
        builder.Property(x => x.IsActive).HasColumnName("IsActive");

        builder.HasOne<Password>(nameof(Password));
        builder.HasOne<Address>(nameof(Address));
        builder.HasOne<PersonalDetail>(nameof(PersonalDetail));
        builder.HasOne<Contact>(nameof(Contact));
        builder.Navigation(x => x.Password).AutoInclude();
        builder.Navigation(x => x.Address).AutoInclude();
        builder.Navigation(x => x.PersonalDetail).AutoInclude();
        builder.Navigation(x => x.Contact).AutoInclude();
    }

    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable($"{nameof(Merchant)}s");
        builder.HasKey("_Id");
        builder.Property<Name>("_CompanyName").HasColumnName("CompanyName").HasConversion<string>(x => x, y => new Name(y));
        builder.Property<bool>("_IsActive").HasColumnName("IsActive");

        builder.HasOne<Address>("_Address");
        builder.HasOne<BusinessInfo>("_BusinessInfo");
        builder.Navigation($"_{nameof(Address)}").AutoInclude();
        builder.Navigation($"_{nameof(BusinessInfo)}").AutoInclude();
    }

    public void Configure(EntityTypeBuilder<MerchantRegistration> builder)
    {
        builder.ToTable($"{nameof(MerchantRegistration)}s");
        builder.HasKey("_Id");
        builder.Property<DateTimeUtc>("_RegistrationDate").HasColumnName("RegistrationDate").HasConversion(ValueConverters.FromDateTimeUtc.Convert);
        builder.Property<Name?>("_CompanyName").HasColumnName("CompanyName").HasConversion(x => x ?? string.Empty, y => new Name(y));
        builder.Property<MerchantRegistrationStatus>("_Status").HasColumnName("Status").HasConversion<string>(x => x, y => new MerchantRegistrationStatus(y));

        builder.HasOne<Address>("_Address");
        builder.HasOne<BusinessInfo>("_BusinessInfo");
    }

    public void Configure(EntityTypeBuilder<UserRegistration> builder)
    {
        builder.ToTable($"{nameof(UserRegistration)}s");
        builder.HasKey("_Id");
        builder.Property<DateTimeUtc>("_RegistrationDate").HasColumnName("RegistrationDate").HasConversion(ValueConverters.FromDateTimeUtc.Convert);
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