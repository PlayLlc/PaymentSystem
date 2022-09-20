using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Play.AuthenticationManagement.Identity.Models;
using System.Security.Claims;

namespace Play.AuthenticationManagement.Identity.Data.DataSeed;

internal class IdentityDbContextSeedData
{
    private readonly DbContext _Context;

    public IdentityDbContextSeedData(DbContext context)
    {
        _Context = context;
    }

    public async Task SeedDefaultRoles()
    {
        var roleStore = new RoleStore<IdentityRole>(_Context);

        if (!_Context.Roles.Any(r => r.Name == "Admin"))
        {
            await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "admin" });
        }

        if (!_Context.Roles.Any(r => r.Name == "Client"))
        {
            await roleStore.CreateAsync(new IdentityRole { Name = "Client", NormalizedName = "client" });
        }

        await _Context.SaveChangesAsync();
    }

    public async Task SeedDefaultAdmin()
    {
        var defaultAdmin = new User
        {
            UserName = "admin",
            NormalizedUserName = "admin",
            Email = "adminemail@email.com",
            NormalizedEmail = "adminemail@email.com",
            FirstName = "Play",
            LastName = "Admin",
            EmailConfirmed = true,
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString(),
            LastPasswordChangedDate = DateTime.MinValue
        };

        if (!_Context.Users.Any(u => u.UserName == defaultAdmin.UserName))
        {
            var password = new PasswordHasher<User>();
            var hashed = password.HashPassword(defaultAdmin, "P@ssword123");
            defaultAdmin.PasswordHash = hashed;
            var userStore = new UserStore<User>(_Context);
            await userStore.CreateAsync(defaultAdmin);
            await userStore.AddToRoleAsync(defaultAdmin, "admin");

            //add some claims for our admin
            var result = userStore.AddClaimsAsync(defaultAdmin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Admin"),
                new Claim(JwtClaimTypes.GivenName, "Play Admin"),
                new Claim(JwtClaimTypes.FamilyName, "Play Family"),
                new Claim(JwtClaimTypes.Email, "playadmin@paywithplay.com"),
                new Claim(JwtClaimTypes.WebSite, "https://notused.com"),
            });

        }

        await _Context.SaveChangesAsync();
    }
}
