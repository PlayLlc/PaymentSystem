using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Globalization.Time;
using Play.Randoms;

using static System.Net.Mime.MediaTypeNames;

namespace Play.Accounts.Persistence.Sql.Persistence;

public class UserIdentityDbSeeder
{
    #region Instance Values

    private readonly UserIdentityDbContext _Context;
    private readonly IHashPasswords _PasswordHasher;

    #endregion

    #region Constructor

    public UserIdentityDbSeeder(UserIdentityDbContext context, IHashPasswords passwordHasher)
    {
        _Context = context;
        _PasswordHasher = passwordHasher;
    }

    #endregion

    #region Instance Members

    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="DbUpdateException"></exception>
    public async Task Seed(UserManager<UserIdentity> userManager, RoleStore<RoleIdentity> roleStore)
    {
        if (!await roleStore.Roles.AnyAsync().ConfigureAwait(false))
            await SeedRoles(roleStore).ConfigureAwait(false);

        if (await userManager.Users.AnyAsync().ConfigureAwait(false))
            return;

        UserIdentity user = await AddSuperAdmin(userManager).ConfigureAwait(false);

        await AddClaims(userManager, user).ConfigureAwait(false);

        await _Context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <exception cref="DbUpdateException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    private async Task SeedRoles(RoleStore<RoleIdentity> roleStore)
    {
        if (await roleStore.Roles.AnyAsync().ConfigureAwait(false))
            return;

        List<RoleIdentity> roles = UserRoles.Empty.GetAll()
            .Select(a => new RoleIdentity
            {
                Id = a,
                Name = a,
                NormalizedName = ((string) a).ToUpper()
            })
            .ToList();

        foreach (RoleIdentity role in roles)
            await roleStore.CreateAsync(role).ConfigureAwait(false);
    }

    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="DbUpdateException"></exception>
    private async Task<UserIdentity> AddSuperAdmin(UserManager<UserIdentity> userManager)
    {
        Address address = new(new AddressDto()
        {
            Id = Randomize.AlphaNumericSpecial.String(20),
            StreetAddress = "1234 Radio Shack Rd",
            ApartmentNumber = "42069",
            City = "Dallas",
            State = "Texas",
            Zipcode = "75036"
        });

        PersonalDetail personalDetail = new(new PersonalDetailDto()
        {
            Id = Randomize.AlphaNumericSpecial.String(20),
            DateOfBirth = new DateTimeUtc(1969, 4, 20),
            LastFourOfSocial = "6969"
        });

        Contact contact = new(new ContactDto()
        {
            Id = Randomize.AlphaNumericSpecial.String(20),
            Email = "test@aol.com",
            FirstName = "Ralph",
            LastName = "Nader",
            Phone = "4204206969"
        });

        string clearTextPassword = "Password1!";
        string passwordId = Randomize.AlphaNumericSpecial.String(20);
        string userId = Randomize.AlphaNumericSpecial.String(20);
        string merchantId = Randomize.AlphaNumericSpecial.String(20);
        string terminalId = Randomize.AlphaNumericSpecial.String(20);

        string hashedPassword = _PasswordHasher.GeneratePasswordHash(clearTextPassword);
        Password password = new Password(passwordId, hashedPassword, DateTimeUtc.Now);

        // TODO: Create Merchant

        UserIdentity superAdmin = new UserIdentity(userId, merchantId, terminalId, password, contact, address, personalDetail)
        {
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        await userManager.CreateAsync(superAdmin).ConfigureAwait(false);
        await userManager.AddToRoleAsync(superAdmin, nameof(UserRoles.SuperAdmin));
        await _Context.SaveChangesAsync().ConfigureAwait(false);

        return superAdmin;
    }

    private async Task AddClaims(UserManager<UserIdentity> userManager, UserIdentity user)
    {
        //add some claims for our admin
        await userManager.AddClaimsAsync(user,
            new Claim[]
            {
                new(JwtClaimTypes.Name, user.Contact.GetFullName()), new(JwtClaimTypes.GivenName, user.Contact.FirstName),
                new(JwtClaimTypes.FamilyName, user.Contact.LastName), new(JwtClaimTypes.Email, user.Contact.Email)
            });
    }

    #endregion
}