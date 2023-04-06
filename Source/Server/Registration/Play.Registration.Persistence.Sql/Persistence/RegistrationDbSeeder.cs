using Play.Registration.Domain.Services;

namespace Play.Registration.Persistence.Sql.Persistence;

public class RegistrationDbSeeder
{
    #region Instance Values

    private readonly RegistrationDbContext _Context;
    private readonly IHashPasswords _PasswordHasher;

    #endregion

    #region Constructor

    public RegistrationDbSeeder(RegistrationDbContext context, IHashPasswords passwordHasher)
    {
        _Context = context;
        _PasswordHasher = passwordHasher;
    }

    #endregion

    ///// <exception cref="OperationCanceledException"></exception>
    ///// <exception cref="DbUpdateException"></exception>
    //public async Task Seed(UserManager<UserIdentity> userManager, RoleStore<RoleIdentity> roleStore)
    //{
    //    if (!await roleStore.Roles.AnyAsync().ConfigureAwait(false))
    //        await SeedRoles(roleStore).ConfigureAwait(false);

    //    if (await userManager.Users.AnyAsync().ConfigureAwait(false))
    //        return;

    //    UserIdentity user = await AddSuperAdmin(userManager).ConfigureAwait(false);

    //    await AddClaims(userManager, user).ConfigureAwait(false);

    //    await _Context.SaveChangesAsync().ConfigureAwait(false);
    //}

    ///// <exception cref="DbUpdateException"></exception>
    ///// <exception cref="OperationCanceledException"></exception>
    //private async Task SeedRoles(RoleStore<RoleIdentity> roleStore)
    //{
    //    if (await roleStore.Roles.AnyAsync().ConfigureAwait(false))
    //        return;

    //    List<RoleIdentity> roles = UserRoles.Empty.GetAll()
    //        .Select(a => new RoleIdentity
    //        {
    //            Id = a,
    //            Name = a,
    //            NormalizedName = ((string) a).ToUpper()
    //        })
    //        .ToList();

    //    foreach (RoleIdentity role in roles)
    //        await roleStore.CreateAsync(role).ConfigureAwait(false);
    //}

    ///// <exception cref="OperationCanceledException"></exception>
    ///// <exception cref="DbUpdateException"></exception>
    //private async Task<UserIdentity> AddSuperAdmin(UserManager<UserIdentity> userManager)
    //{
    //    Address address = new(new AddressDto
    //    {
    //        Id = Randomize.AlphaNumericSpecial.String(20),
    //        StreetAddress = "1234 Radio Shack Rd",
    //        ApartmentNumber = "42069",
    //        City = "Dallas",
    //        State = "Texas",
    //        Zipcode = "75036"
    //    });

    //    PersonalDetail personalDetail = new(new PersonalDetailDto
    //    {
    //        Id = Randomize.AlphaNumericSpecial.String(20),
    //        DateOfBirth = new DateTimeUtc(1969, 4, 20),
    //        LastFourOfSocial = "6969"
    //    });

    //    Contact contact = new(new ContactDto
    //    {
    //        Id = Randomize.AlphaNumericSpecial.String(20),
    //        Email = "test@aol.com",
    //        FirstName = "Ralph",
    //        LastName = "Nader",
    //        Phone = "4204206969"
    //    });

    //    string clearTextPassword = "Password1!";
    //    string userId = Randomize.AlphaNumericSpecial.String(20);
    //    string passwordId = userId;
    //    string merchantId = Randomize.AlphaNumericSpecial.String(20);
    //    string terminalId = Randomize.AlphaNumericSpecial.String(20);

    //    string hashedPassword = _PasswordHasher.GeneratePasswordHash(clearTextPassword);
    //    Password password = new(passwordId, hashedPassword, DateTimeUtc.Now);

    //    // TODO: Create Merchant

    //    UserIdentity superAdmin = new(userId, merchantId, terminalId, password, contact, address, personalDetail)
    //    {
    //        EmailConfirmed = true,
    //        PhoneNumberConfirmed = true
    //    };

    //    await userManager.CreateAsync(superAdmin).ConfigureAwait(false);
    //    await userManager.AddToRoleAsync(superAdmin, nameof(UserRoles.SuperAdmin));
    //    await _Context.SaveChangesAsync().ConfigureAwait(false);

    //    return superAdmin;
    //}
}