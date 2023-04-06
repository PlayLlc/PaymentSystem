using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.ValueObjects;
using Play.Persistence.Sql;
using Play.Registration.Persistence.Sql.Entities;

namespace Play.Registration.Persistence.Sql.Configuration;

// You can configure a navigation in the model to be included every time the entity is loaded from the database using AutoInclude method
// https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
internal class IdentityEntityConfiguration : IEntityTypeConfiguration<UserRegistration>, IEntityTypeConfiguration<MerchantRegistration>,
    IEntityTypeConfiguration<Merchant>, IEntityTypeConfiguration<UserIdentity>
{
    #region Instance Members

    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.ToTable($"{nameof(User)}s").Property(x => x.Id).ValueGeneratedOnAdd();
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
        builder.ToTable($"{nameof(Merchant)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);
        builder.Property<Name>("_CompanyName").HasColumnName("CompanyName");
        builder.Property<bool>("_IsActive").HasColumnName("IsActive");
        builder.HasOne<Merchant, Address, SimpleStringId>("_Address", "AddressId");
        builder.HasOne<Merchant, BusinessInfo, SimpleStringId>("_BusinessInfo", "BusinessInfoId");
        builder.Navigation($"_{nameof(Address)}").AutoInclude();
        builder.Navigation($"_{nameof(BusinessInfo)}").AutoInclude();
    }

    public void Configure(EntityTypeBuilder<MerchantRegistration> builder)
    {
        builder.ToTable($"{nameof(MerchantRegistration)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);
        builder.Property<DateTimeUtc>("_RegistrationDate").HasColumnName("RegistrationDate").ValueGeneratedOnAdd();
        builder.Property<Name>("_CompanyName").HasColumnName("CompanyName");
        builder.Property<MerchantRegistrationStatus>("_Status").HasColumnName("Status");
        builder.HasOne<MerchantRegistration, Address, SimpleStringId>("_Address", "AddressId");
        builder.HasOne<MerchantRegistration, BusinessInfo, SimpleStringId>("_BusinessInfo", "BusinessInfoId");
    }

    public void Configure(EntityTypeBuilder<UserRegistration> builder)
    {
        builder.ToTable($"{nameof(UserRegistration)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);
        builder.Property<SimpleStringId>("_MerchantId").HasColumnName("MerchantId").ValueGeneratedOnAdd();
        builder.Property<DateTimeUtc>("_RegistrationDate").HasColumnName("RegistrationDate").ValueGeneratedOnAdd();
        builder.Property<string>("_Username").HasColumnName("Username");
        builder.Property<string>("_HashedPassword").HasColumnName("HashedPassword");
        builder.Property<bool>("_HasEmailBeenVerified").HasColumnName("HasEmailBeenVerified");
        builder.Property<bool>("_HasPhoneBeenVerified").HasColumnName("HasPhoneBeenVerified");
        builder.Property<UserRegistrationStatus>("_Status").HasColumnName("Status");

        builder.HasOne<UserRegistration, Address, SimpleStringId>("_Address", "AddressId");
        builder.HasOne<UserRegistration, Contact, SimpleStringId>("_Contact", "ContactId");
        builder.HasOne<UserRegistration, PersonalDetail, SimpleStringId>("_PersonalDetail", "PersonalDetailId");
        builder.HasOne<UserRegistration, EmailConfirmationCode, SimpleStringId>("_EmailConfirmation", "EmailConfirmationId");
        builder.HasOne<UserRegistration, SmsConfirmationCode, SimpleStringId>("_SmsConfirmation", "SmsConfirmationId");
    }

    #endregion
}