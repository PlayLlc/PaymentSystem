using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Play.Accounts.Domain.Entities;
using Play.Accounts.Persistence.Sql.Entities;

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
        builder.Entity<Address>().ToTable("Addresses").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Address>().HasKey(x => x.Id);

        builder.Entity<Contact>().ToTable("Contacts").Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Contact>().HasKey(x => x.Id);

        RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder) builder.Entity<PersonalDetail>(), "PersonalDetails")
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder.Entity<PersonalDetail>().HasKey(x => x.Id);

        builder.Entity<UserIdentity>().ToTable("UserIdentities");
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