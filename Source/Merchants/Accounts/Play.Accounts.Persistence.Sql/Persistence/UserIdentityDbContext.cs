using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Configuration;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Randoms;

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
        var accountEntityConfiguration = new AccountsEntityConfiguration();

        // Enums
        builder.Entity<BusinessTypes>().HasData(BusinessTypes.Empty.GetAll().Select(e => new BusinessType(e)));
        builder.Entity<MerchantCategoryCode>().HasData(MerchantCategoryCodes.Empty.GetAll().Select(e => new MerchantCategoryCode(e)));
        builder.Entity<MerchantRegistrationStatuses>().HasData(MerchantRegistrationStatuses.Empty.GetAll().Select(e => new MerchantRegistrationStatus(e)));
        builder.Entity<State>().HasData(States.Empty.GetAll().Select(e => new State(e)));
        builder.Entity<UserRegistrationStatus>().HasData(UserRegistrationStatuses.Empty.GetAll().Select(e => new UserRegistrationStatus(e)));

        // Entities 
        builder.Entity<Address>().ToTable($"{nameof(Address)}es").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Address>().HasKey(x => x.Id);
        builder.Entity<Address>().Property(nameof(Zipcode)).HasConversion<string>();
        builder.Entity<Address>().Property(nameof(State)).HasConversion<string>();

        builder.Entity<BusinessInfo>().ToTable($"{nameof(BusinessInfo)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<BusinessInfo>().HasKey(x => x.Id);
        builder.Entity<BusinessInfo>().Property(x => x.BusinessType).HasConversion<string>();
        builder.Entity<BusinessInfo>().Property(x => x.MerchantCategoryCode).HasConversion<ushort>();

        builder.Entity<ConfirmationCode>().ToTable($"{nameof(ConfirmationCode)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<ConfirmationCode>().HasKey(x => x.Id);
        builder.Entity<ConfirmationCode>().Property(x => x.SentDate).HasConversion<DateTime>();

        builder.Entity<Contact>().ToTable($"{nameof(Contact)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Contact>().HasKey(x => x.Id);
        builder.Entity<Contact>().Property(x => x.Email).HasConversion<string>();
        builder.Entity<Contact>().Property(x => x.FirstName).HasConversion<string>();
        builder.Entity<Contact>().Property(x => x.LastName).HasConversion<string>();
        builder.Entity<Contact>().Property(x => x.Email).HasConversion<string>();
        builder.Entity<Contact>().Property(x => x.Phone).HasConversion<string>();

        builder.Entity<Password>().ToTable($"{nameof(Password)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Password>().HasKey(x => x.Id);

        builder.Entity<PersonalDetail>().ToTable($"{nameof(PersonalDetail)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<PersonalDetail>().HasKey(x => x.Id);
        builder.Entity<PersonalDetail>().Property(x => x.DateOfBirth).HasConversion<DateTime>();

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