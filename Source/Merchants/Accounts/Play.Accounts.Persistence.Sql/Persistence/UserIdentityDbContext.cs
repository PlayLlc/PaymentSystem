using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Configuration;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Domain.ValueObjects;
using Play.Globalization.Time;

using System.Diagnostics;

namespace Play.Accounts.Persistence.Sql.Persistence;

public class UserIdentityDbContext : IdentityDbContext<UserIdentity, RoleIdentity, string>
{
    #region Constructor

    public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
    { }

    #endregion

    #region Instance Members

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Debugger.Launch();
        AccountsEntityConfiguration accountEntityConfiguration = new AccountsEntityConfiguration();

        // Enums
        //builder.Entity<BusinessType>().HasData(BusinessTypes.Empty.GetAll().Select(e => new BusinessType(e)));
        //builder.Entity<MerchantCategoryCode>().HasData(MerchantCategoryCodes.Empty.GetAll().Select(e => new MerchantCategoryCode(e)));
        //builder.Entity<MerchantRegistrationStatus>().HasData(MerchantRegistrationStatuses.Empty.GetAll().Select(e => new MerchantRegistrationStatus(e)));
        //builder.Entity<State>().HasData(States.Empty.GetAll().Select(e => new State(e)));
        //builder.Entity<UserRegistrationStatus>().HasData(UserRegistrationStatuses.Empty.GetAll().Select(e => new UserRegistrationStatus(e)));

        // Entities 
        builder.Entity<Address>().ToTable($"{nameof(Address)}es").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Address>().HasKey(x => x.Id);
        builder.Entity<Address>().Property<Zipcode>(nameof(Zipcode)).HasColumnName(nameof(Zipcode)).HasConversion<string>(x => x.Value, y => new Zipcode(y));
        builder.Entity<Address>().Property<State>(nameof(State)).HasConversion<string>(x => x, y => new State(y));

        builder.Entity<BusinessInfo>().ToTable($"{nameof(BusinessInfo)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<BusinessInfo>().HasKey(x => x.Id);
        builder.Entity<BusinessInfo>().Property<BusinessType>(x => x.BusinessType).HasConversion<string>(x => x, y => new BusinessType(y));

        builder.Entity<BusinessInfo>()
            .Property<MerchantCategoryCode>(x => x.MerchantCategoryCode)
            .HasConversion<ushort>(x => x, y => new MerchantCategoryCode(y));

        builder.Entity<ConfirmationCode>().ToTable($"{nameof(ConfirmationCode)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<ConfirmationCode>().HasKey(x => x.Id);
        builder.Entity<ConfirmationCode>().Property<DateTimeUtc>(x => x.SentDate).HasConversion<DateTime>(x => x, y => new DateTimeUtc(y));

        builder.Entity<Contact>().ToTable($"{nameof(Contact)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Contact>().HasKey(x => x.Id);
        builder.Entity<Contact>().Property<Email>(x => x.Email).HasConversion<string>(x => x, y => new Email(y));
        builder.Entity<Contact>().Property<Name>(x => x.FirstName).HasConversion<string>(x => x, y => new Name(y));
        builder.Entity<Contact>().Property<Name>(x => x.LastName).HasConversion<string>(x => x, y => new Name(y));
        builder.Entity<Contact>().Property<Email>(x => x.Email).HasConversion<string>(x => x, y => new Email(y));
        builder.Entity<Contact>().Property<Phone>(x => x.Phone).HasConversion<string>(x => x, y => new Phone(y));

        builder.Entity<Password>().ToTable($"{nameof(Password)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Password>().HasKey(x => x.Id);
        builder.Entity<Password>().Property<DateTimeUtc>(x => x.CreatedOn).HasConversion<DateTime>(x => x, y => new DateTimeUtc(y));

        builder.Entity<PersonalDetail>().ToTable($"{nameof(PersonalDetail)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<PersonalDetail>().HasKey(x => x.Id);
        builder.Entity<PersonalDetail>().Property<DateTimeUtc>(x => x.DateOfBirth).HasConversion<DateTime>(x => x, y => new DateTimeUtc(y));

        // Aggregates 
        accountEntityConfiguration.Configure(builder.Entity<UserRegistration>());
        accountEntityConfiguration.Configure(builder.Entity<MerchantRegistration>());
        accountEntityConfiguration.Configure(builder.Entity<Merchant>());
        accountEntityConfiguration.Configure(builder.Entity<UserIdentity>());

        builder.Entity<RoleIdentity>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserRole<string>>()
        .ToTable("UserRoles")
        .HasKey(k => new
        {
            k.UserId,
            k.RoleId
        });
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasKey(k => k.UserId);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims").HasKey(k => k.Id);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(k => k.UserId);
        builder.Entity<IdentityProviders>();
    }

    #endregion
}