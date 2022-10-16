using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Enums;

namespace Play.Accounts.Persistence.Sql.Persistence;

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
    public async Task Seed(ConfigurationManager manager, UserManager<UserIdentity> userManager, RoleStore<Role> roleStore)
    {
        //SendGridConfig? config = manager.GetSection(nameof(SendGridConfig)).Get<SendGridConfig>();
        var a = new EmailAccountVerifier();
        await a.SendVerificationCode().ConfigureAwait(false);

        if (!await roleStore.Roles.AnyAsync().ConfigureAwait(false))
            await SeedRoles(roleStore).ConfigureAwait(false);

        if (await userManager.Users.AnyAsync().ConfigureAwait(false))
            return;

        var user = await AddSuperAdmin(userManager).ConfigureAwait(false);

        await AddClaims(userManager, user).ConfigureAwait(false);

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
    /// <exception cref="DbUpdateException"></exception>
    private async Task<UserIdentity> AddSuperAdmin(UserManager<UserIdentity> userManager)
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
            Email = "test@aol.com",
            FirstName = "Ralph",
            LastName = "Nader",
            Phone = "4204206969"
        };

        string password = "Password1!";
        UserIdentity superAdmin = new UserIdentity(contactInfo, address, personalInfo)
        {
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        await userManager.CreateAsync(superAdmin, password).ConfigureAwait(false);
        await userManager.AddToRoleAsync(superAdmin, nameof(RoleTypes.SuperAdmin));
        await _Context.SaveChangesAsync().ConfigureAwait(false);

        return superAdmin;
    }

    private async Task AddClaims(UserManager<UserIdentity> userManager, UserIdentity user)
    {
        //add some claims for our admin
        await userManager.AddClaimsAsync(user,
            new Claim[]
            {
                new(JwtClaimTypes.Name, "Admin"), new(JwtClaimTypes.GivenName, "Play Admin"), new(JwtClaimTypes.FamilyName, "Play Family"),
                new(JwtClaimTypes.Email, "playadmin@paywithplay.com"), new(JwtClaimTypes.WebSite, "https://notused.com")
            });
    }

    #endregion
}