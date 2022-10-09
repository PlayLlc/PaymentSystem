﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Play.Identity.Api.Identity.Entities;

namespace Play.Identity.Api.Identity.Persistence
{
    public class UserIdentityDbContext : IdentityDbContext<UserIdentity, Role, string>
    {
        #region Constructor

        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
        { }

        #endregion

        #region Instance Members

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserIdentity>().ToTable("UserIdentities");
            builder.Entity<Role>().ToTable("Roles");
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
}