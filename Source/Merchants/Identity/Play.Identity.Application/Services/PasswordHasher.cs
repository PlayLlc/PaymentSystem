﻿using Microsoft.AspNetCore.Identity;

using Play.Domain.Common.Entities;
using Play.Globalization.Time;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Services;
using Play.Identity.Persistence.Sql.Entities;

namespace Play.Identity.Application.Services;

public class PasswordHasher : IHashPasswords
{
    #region Instance Values

    private readonly IPasswordHasher<UserIdentity> _PasswordHasher;

    private readonly UserIdentity _MockUser;

    #endregion

    #region Constructor

    public PasswordHasher(UserManager<UserIdentity> userManager)
    {
        _MockUser = GetMockUser();
        _PasswordHasher = userManager.PasswordHasher;
    }

    #endregion

    #region Instance Members

    /// <WARNING>
    ///     We're using the default password hasher provided by Microsoft.AspNetCore.Identity in .NET 6.0. The concrete
    ///     implementation of the IPasswordHasher does not use the IdentityUser in any way to generate the password hash or to
    ///     validate the password hash. That means we can safely pass either a real IdentityUser or a mocked IdentityUser to
    ///     the default implementation and the outcome for generating and validating the passwords will be the same. If we
    ///     update our password provider or create a default implementation then we will need to ensure that passing a mocked
    ///     IdentityUser will not cause a security risk
    /// </WARNING>
    /// <returns></returns>
    private static UserIdentity GetMockUser()
    {
        Password password1 = new Password("12345678901234567890", "DEFAULTOBJECT", DateTimeUtc.Now);
        Contact coontact = new Contact("12345678901234567890", "DEFAULTOBJECT", "DEFAULTOBJECT", "5555555555", "DEFAULTOBJECT@DEFAULTOBJECT.com");
        Address address = new Address("12345678901234567890", "DEFAULTOBJECT", "55555", "Texas", "DEFAULTOBJECT");
        PersonalDetail personalDetail = new PersonalDetail("12345678901234567890", "0000", DateTimeUtc.Now);
        UserIdentity userIdentity = new UserIdentity("12345678901234567890", "DEFAULTOBJECT", "DEFAULTOBJECT", password1, coontact, address, personalDetail);

        return userIdentity;
    }

    public string GeneratePasswordHash(string password)
    {
        return _PasswordHasher.HashPassword(_MockUser, password);
    }

    public bool ValidateHashedPassword(string hashedPassword, string clearTextPassword)
    {
        PasswordVerificationResult result = _PasswordHasher.VerifyHashedPassword(_MockUser, hashedPassword, clearTextPassword);

        if (result == PasswordVerificationResult.Success)
            return true;

        return false;
    }

    #endregion
}