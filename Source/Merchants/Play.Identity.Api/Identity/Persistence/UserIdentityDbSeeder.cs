using IdentityModel;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Play.Identity.Api.Identity.Entities;

using System.Security.Claims;

using Play.Identity.Api.Identity.Enums;

using Duende.IdentityServer.Test;
using Duende.IdentityServer;

using Microsoft.EntityFrameworkCore;

namespace Play.Identity.Api.Identity.Persistence
{
    internal class UserIdentityDbSeeder
    {
        #region Instance Values

        private readonly UserIdentityDbContext _Context;

        #endregion

        #region Constructor

        public UserIdentityDbSeeder(UserIdentityDbContext context)
        {
            _Context = context;
        }

        #endregion

        #region Instance Members

        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        public async Task Seed()
        {
            RoleStore<Role> roleStore = new RoleStore<Role>(_Context);
            UserStore<UserIdentity, Role, UserIdentityDbContext> userStore = new UserStore<UserIdentity, Role, UserIdentityDbContext>(_Context);

            if (!await roleStore.Roles.AnyAsync().ConfigureAwait(false))
                await SeedRoles(roleStore).ConfigureAwait(false);

            if (await userStore.Users.AnyAsync().ConfigureAwait(false))
                return;

            var user = await AddSuperAdmin(userStore).ConfigureAwait(false);

            await AddClaims(userStore, user).ConfigureAwait(false);

            await _Context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        private async Task SeedRoles(RoleStore<Role> roleStore)
        {
            if (await roleStore.Roles.AnyAsync().ConfigureAwait(false))
                return;

            List<Role> roles = Enum.GetNames<RoleTypes>()
                .Select(a => new Role
                {
                    Name = a,
                    NormalizedName = a.ToLower()
                })
                .ToList();

            foreach (var role in roles)
                await roleStore.CreateAsync(role).ConfigureAwait(false);
        }

        /// <exception cref="OperationCanceledException"></exception>
        private async Task<UserIdentity> AddSuperAdmin(UserStore<UserIdentity, Role, UserIdentityDbContext> userStore)
        {
            Address address = new()
            {
                StreetAddress = "1234 Radio Shack Rd",
                ApartmentNumber = "42069",
                City = "Dallas",
                State = "Texas",
                Zipcode = "75036"
            };

            PersonalInfo personalInfo = new()
            {
                DateOfBirth = new DateTime(1969, 4, 20),
                LastFourOfSocial = "6969"
            };

            ContactInfo contactInfo = new()
            {
                Email = "test",
                FirstName = "Ralph",
                LastName = "Nader",
                Phone = "4204206969"
            };

            UserIdentity superAdmin = new UserIdentity(contactInfo, address, personalInfo);

            superAdmin.PasswordHash = new PasswordHasher<UserIdentity>().HashPassword(superAdmin, "test");

            await userStore.CreateAsync(superAdmin).ConfigureAwait(false);
            await userStore.AddToRoleAsync(superAdmin, nameof(RoleTypes.SuperAdmin));

            return superAdmin;
        }

        private async Task AddClaims(UserStore<UserIdentity, Role, UserIdentityDbContext> userStore, UserIdentity user)
        {
            //add some claims for our admin
            await userStore.AddClaimsAsync(user,
                new Claim[]
                {
                    new(JwtClaimTypes.Name, "Admin"), new(JwtClaimTypes.GivenName, "Play Admin"), new(JwtClaimTypes.FamilyName, "Play Family"),
                    new(JwtClaimTypes.Email, "playadmin@paywithplay.com"), new(JwtClaimTypes.WebSite, "https://notused.com")
                });
        }

        #endregion
    }
}