using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Configuration;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Accounts.Domain.Enums;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Play.Persistence.Sql;

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
        AccountsEntityConfiguration accountEntityConfiguration = new AccountsEntityConfiguration();

        // Enums

        #region Enums

        builder.Entity<BusinessType>().ToTable($"{nameof(BusinessTypes)}").HasKey(a => a.Value);
        builder.Entity<BusinessType>().HasData(BusinessTypes.Empty.GetAll().Select(e => new BusinessType(e)));

        builder.Entity<MerchantCategoryCode>().ToTable($"{nameof(MerchantCategoryCodes)}").HasKey(a => a.Value);
        builder.Entity<MerchantCategoryCode>().Property(a => a.Name);
        builder.Entity<MerchantCategoryCode>().HasData(MerchantCategoryCodes.Empty.GetAll().Select(e => new MerchantCategoryCode(e)));

        builder.Entity<MerchantRegistrationStatus>().ToTable($"{nameof(MerchantRegistrationStatuses)}").HasKey(a => a.Value);
        builder.Entity<MerchantRegistrationStatus>().HasData(MerchantRegistrationStatuses.Empty.GetAll().Select(e => new MerchantRegistrationStatus(e)));

        builder.Entity<State>().ToTable($"{nameof(States)}").HasKey(a => a.Value);
        builder.Entity<State>().HasData(States.Empty.GetAll().Select(e => new State(e)));

        builder.Entity<UserRegistrationStatus>().ToTable($"{nameof(UserRegistrationStatuses)}").HasKey(a => a.Value);
        builder.Entity<UserRegistrationStatus>().HasData(UserRegistrationStatuses.Empty.GetAll().Select(e => new UserRegistrationStatus(e)));

        #endregion

        #region Entities

        // HACK: Right now we are explicitly declaring each property as a column. How do we automate this to reduce the plumbing

        builder.Entity<Address>().ToTable($"{nameof(Address)}es").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Address>().HasKey(x => x.Id);
        builder.Entity<Address>().Property(x => x.StreetAddress);
        builder.Entity<Address>().Property(x => x.ApartmentNumber);
        builder.Entity<Address>().Property<Zipcode>(nameof(Zipcode)).HasConversion(x => x.Value, y => new Zipcode(y));
        builder.Entity<Address>().Property<State>(nameof(State)).HasConversion<string>(x => x, y => new State(y));
        builder.Entity<Address>().Property(x => x.City);

        builder.Entity<BusinessInfo>().ToTable($"{nameof(BusinessInfo)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<BusinessInfo>().HasKey(x => x.Id);
        builder.Entity<BusinessInfo>().Property(x => x.BusinessType).HasConversion<string>(x => x, y => new BusinessType(y));
        builder.Entity<BusinessInfo>().Property(x => x.MerchantCategoryCode).HasConversion<ushort>(x => x, y => new MerchantCategoryCode(y));

        builder.Entity<Contact>().ToTable($"{nameof(Contact)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Contact>().HasKey(x => x.Id);
        builder.Entity<Contact>().Property(x => x.FirstName).HasConversion<string>(x => x, y => new Name(y));
        builder.Entity<Contact>().Property(x => x.LastName).HasConversion<string>(x => x, y => new Name(y));
        builder.Entity<Contact>().Property(x => x.Phone).HasConversion<string>(x => x, y => new Phone(y));
        builder.Entity<Contact>().Property(x => x.Email).HasConversion<string>(x => x, y => new Email(y));

        builder.Entity<Password>().ToTable($"{nameof(Password)}s").Property(x => x.Id).HasColumnName($"{nameof(User)}Id").ValueGeneratedOnAdd();
        builder.Entity<Password>().Property(x => x.CreatedOn).HasConversion(ValueConverters.FromDateTimeUtc.Convert).ValueGeneratedOnAdd();
        builder.Entity<Password>()
        .HasKey(x => new
        {
            x.Id,
            x.CreatedOn
        });
        builder.Entity<Password>().Property(x => x.HashedPassword).ValueGeneratedOnAdd();

        builder.Entity<PersonalDetail>().ToTable($"{nameof(PersonalDetail)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<PersonalDetail>().HasKey(x => x.Id);
        builder.Entity<PersonalDetail>().Property<string>(x => x.LastFourOfSocial).ValueGeneratedOnAdd();
        builder.Entity<PersonalDetail>().Property(x => x.DateOfBirth).HasConversion(ValueConverters.FromDateTimeUtc.Convert).ValueGeneratedOnAdd();

        builder.Entity<ConfirmationCode>().ToTable($"{nameof(ConfirmationCode)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<ConfirmationCode>().HasKey(x => x.Id);
        builder.Entity<ConfirmationCode>().Property(x => x.SentDate).HasConversion(ValueConverters.FromDateTimeUtc.Convert).ValueGeneratedOnAdd();
        builder.Entity<ConfirmationCode>().Property(x => x.Code).ValueGeneratedOnAdd();

        #endregion

        #region Aggregates

        // Aggregates 
        accountEntityConfiguration.Configure(builder.Entity<UserRegistration>());
        accountEntityConfiguration.Configure(builder.Entity<MerchantRegistration>());
        accountEntityConfiguration.Configure(builder.Entity<Merchant>());
        accountEntityConfiguration.Configure(builder.Entity<UserIdentity>());

        #endregion

        #region Identity

        builder.Entity<RoleIdentity>()
        .ToTable("Roles")
        .HasData(Domain.Enums.UserRoles.Empty.GetAll()
        .Select(e => new RoleIdentity(e.Name)
        {
            Id = e.Name,
            Name = e.Name,
            NormalizedName = e.Name.ToUpper()
        }));

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

        #endregion
    }

    #endregion
}