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
        builder.ToTable("UserRegistrations", "users");

        builder.HasKey(x => x.GetId());

        builder.Property<DateTime>("_RegisteredDate").HasColumnName("RegisteredDate");
        builder.Property<DateTime>("_ConfirmedDate").HasColumnName("ConfirmedDate");
        builder.Property<DateTime>("_DateOfBirth").HasColumnName("_DateOfBirth");

        builder.OwnsOne<Address>("_Address", b =>
        {
            b.WithOwner().HasPrincipalKey(x => x.GetId());
            b.ToTable(@"Addresses", "users");
            b.Property(x => x.StreetAddress).HasColumnName("StreetAddress");
            b.Property(x => x.ApartmentNumber).HasColumnName("ApartmentNumber");
            b.Property(x => x.City).HasColumnName("City");
            b.Property(x => x.StateAbbreviation).HasColumnName("State");
            b.Property(x => x.Zipcode).HasColumnName("Zipcode");
        });

        builder.OwnsOne<Contact>("_ContactInfo", b =>
        {
            b.WithOwner().HasPrincipalKey(x => x.GetId());
            b.ToTable("ContactInfo", "users");
            b.Property(x => x.FirstName).HasColumnName("FirstName");
            b.Property(x => x.LastName).HasColumnName("LastName");
            b.Property(x => x.Phone).HasColumnName("Phone");
            b.Property(x => x.Email).HasColumnName("Email");
        });

        builder.OwnsOne<RegistrationStatuses>("_Status", b =>
        {
            b.Property(x => x).HasColumnName("Status");
        });
    }

    #endregion
}