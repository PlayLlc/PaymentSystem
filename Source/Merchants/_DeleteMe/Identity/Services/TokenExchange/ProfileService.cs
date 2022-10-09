using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;

using System.Security.Claims;

using _DeleteMe.Identity.Entities;

using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Extensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace _DeleteMe.Identity.Services.TokenExchange
{
    // The IProfileService lets IdentityServer know what claims to include in tokens for a user. 
    //public class ProfileService : ProfileService<UserIdentity>
    //{
    //    #region Instance Values

    //    private readonly UserManager<UserIdentity> _UserManager;
    //    private readonly IUserClaimsPrincipalFactory<UserIdentity> _ClaimsFactory;

    //    #endregion

    //    #region Constructor

    //    public ProfileService(UserManager<UserIdentity> userManager, IUserClaimsPrincipalFactory<UserIdentity> claimsFactory) : base(userManager, claimsFactory)
    //    {
    //        _UserManager = userManager;
    //        _ClaimsFactory = claimsFactory;
    //    }

    //    public ProfileService(
    //        UserManager<UserIdentity> userManager, IUserClaimsPrincipalFactory<UserIdentity> claimsFactory, ILogger<ProfileService<UserIdentity>> logger) :
    //        base(userManager, claimsFactory, logger)
    //    {
    //        _UserManager = userManager;
    //        _ClaimsFactory = claimsFactory;
    //    }

    //    #endregion

    //    #region Instance Members

    //    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    //    {
    //        // OPTION 1B: load claims from the user database this adds any claims that were requested from the claims in the user store
    //        UserIdentity user = await _UserManager.GetUserAsync(context.Subject);

    //        if (user is null)
    //            return;

    //        _ = await _ClaimsFactory.CreateAsync(user);

    //        context.AddRequestedClaims(context.Subject.Claims);

    //        IList<Claim>? userClaims = await _UserManager.GetClaimsAsync(user).ConfigureAwait(false);
    //        context.AddRequestedClaims(userClaims);
    //    }

    //    public async Task IsActiveAsync(IsActiveContext context)
    //    {
    //        var sub = context.Subject.GetSubjectId();
    //        var user = await _UserManager.FindByIdAsync(sub).ConfigureAwait(false);
    //        context.IsActive = user != null;
    //    }

    //    #endregion
    //}
}