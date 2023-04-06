using Microsoft.EntityFrameworkCore;

using Play.Domain.Common.Entities;
using Play.Domain.Common.Enums;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.ValueObjects;
using Play.Persistence.Sql;
using Play.Registration.Persistence.Sql.Configuration;
using Play.Registration.Persistence.Sql.Entities;

namespace Play.Registration.Persistence.Sql.Persistence;

public sealed class RegistrationDbContext : DbContext
{
    #region Constructor

    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    #endregion

    #region Instance Members

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ValueConverters.ConfigureCommonConverters(configurationBuilder);
        DomainValueConverters.ConfigureCommonConverters(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        IdentityEntityConfiguration identityEntityConfiguration = new();

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
        builder.Entity<Address>().Property<Zipcode>(nameof(Zipcode));
        builder.Entity<Address>().Property<State>(nameof(State));
        builder.Entity<Address>().Property(x => x.City);

        builder.Entity<BusinessInfo>().ToTable($"{nameof(BusinessInfo)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<BusinessInfo>().HasKey(x => x.Id);
        builder.Entity<BusinessInfo>().Property(x => x.BusinessType).HasConversion<string>(x => x, y => new BusinessType(y));
        builder.Entity<BusinessInfo>().Property(x => x.MerchantCategoryCode).HasConversion<ushort>(x => x, y => new MerchantCategoryCode(y));

        builder.Entity<Contact>().ToTable($"{nameof(Contact)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Contact>().HasKey(x => x.Id);
        builder.Entity<Contact>().Property(x => x.FirstName);
        builder.Entity<Contact>().Property(x => x.LastName);
        builder.Entity<Contact>().Property(x => x.Phone);
        builder.Entity<Contact>().Property(x => x.Email);

        builder.Entity<Password>().ToTable($"{nameof(Password)}s").Property(x => x.Id).HasColumnName($"{nameof(User)}Id").ValueGeneratedOnAdd();
        builder.Entity<Password>().HasKey(x => x.Id);
        builder.Entity<Password>().Property(x => x.CreatedOn);
        builder.Entity<Password>().Property(x => x.HashedPassword);
        builder.Entity<Password>()
        .HasKey(x => new
        {
            x.Id,
            x.CreatedOn
        });

        builder.Entity<PersonalDetail>().ToTable($"{nameof(PersonalDetail)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<PersonalDetail>().HasKey(x => x.Id);
        builder.Entity<PersonalDetail>().Property(x => x.LastFourOfSocial);
        builder.Entity<PersonalDetail>().Property(x => x.DateOfBirth);

        builder.Entity<EmailConfirmationCode>().ToTable($"{nameof(EmailConfirmationCode)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<EmailConfirmationCode>().HasKey(x => x.Id);
        builder.Entity<EmailConfirmationCode>().Property(x => x.SentDate);
        builder.Entity<EmailConfirmationCode>().Property(x => x.Code);

        builder.Entity<SmsConfirmationCode>().ToTable($"{nameof(SmsConfirmationCode)}s").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<SmsConfirmationCode>().HasKey(x => x.Id);
        builder.Entity<SmsConfirmationCode>().Property(x => x.SentDate);
        builder.Entity<SmsConfirmationCode>().Property(x => x.Code);

        #endregion

        #region Aggregates

        // Aggregates 

        identityEntityConfiguration.Configure(builder.Entity<UserRegistration>());
        identityEntityConfiguration.Configure(builder.Entity<MerchantRegistration>());
        identityEntityConfiguration.Configure(builder.Entity<Merchant>());
        identityEntityConfiguration.Configure(builder.Entity<UserIdentity>());

        #endregion

        #region Identity

        builder.Entity<RoleIdentity>()
        .ToTable("Roles")
        .HasData(UserRoles.Empty.GetAll()
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