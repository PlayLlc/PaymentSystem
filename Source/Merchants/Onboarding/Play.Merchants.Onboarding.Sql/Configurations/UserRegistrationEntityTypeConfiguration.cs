using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.UserRegistration;
using Play.Merchants.Onboarding.Domain.Users;

namespace Play.Merchants.Onboarding.Sql.Configurations
{
    internal class UserRegistrationEntityTypeConfiguration : IEntityTypeConfiguration<UserRegistration>
    {
        #region Instance Members

        public void Configure(EntityTypeBuilder<UserRegistration> builder)
        {
            builder.ToTable("UserRegistrations", "users");

            builder.HasKey(x => x.Id!.Id);

            builder.Property<DateTimeUtc>("_RegisteredDate").HasColumnName("RegisteredDate");
            builder.Property<DateTimeUtc>("_ConfirmedDate").HasColumnName("ConfirmedDate");
            builder.Property<DateTimeUtc>("_DateOfBirth").HasColumnName("_DateOfBirth");

            builder.OwnsOne<Address>("_Address", b =>
            {
                b.WithOwner().HasPrincipalKey(x => x.Id!.Id);
                b.ToTable(@"Addresses", "users");
                b.Property(x => x.StreetAddress).HasColumnName("StreetAddress");
                b.Property(x => x.ApartmentNumber).HasColumnName("ApartmentNumber");
                b.Property(x => x.City).HasColumnName("City");
                b.Property(x => x.State).HasColumnName("State");
                b.Property(x => x.Zipcode).HasColumnName("Zipcode");
            });

            builder.OwnsOne<ContactInfo>("_ContactInfo", b =>
            {
                b.WithOwner().HasPrincipalKey(x => x.Id!.Id);
                b.ToTable("ContactInfo", "users");
                b.Property(x => x.FirstName).HasColumnName("FirstName");
                b.Property(x => x.LastName).HasColumnName("LastName");
                b.Property(x => x.Phone).HasColumnName("Phone");
                b.Property(x => x.Email).HasColumnName("Email");
            });

            builder.OwnsOne<UserRegistrationStatus>("_Status", b =>
            {
                b.Property(x => x.Value).HasColumnName("Status");
            });
        }

        #endregion
    }
}