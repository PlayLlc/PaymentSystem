using System.Security.Claims;

using Duende.IdentityServer;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Play.Identity.Contracts.Dtos;
using Play.Identity.Persistence.Sql.Entities;
using Play.Mvc.Attributes;

namespace Play.Identity.Api.Controllers;

[SwaggerIgnore]
[ApiController]
[Route("[controller]/[action]")]
public class ExternalController : Controller
{
    #region Instance Values

    private readonly IIdentityServerInteractionService _Interaction;
    private readonly ILogger<ExternalController> _Logger;
    private readonly UserManager<UserIdentity> _UserManager;

    #endregion

    #region Constructor

    public ExternalController(IIdentityServerInteractionService interaction, ILogger<ExternalController> logger, UserManager<UserIdentity> userManager)
    {
        _Interaction = interaction;
        _Logger = logger;
        _UserManager = userManager;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     initiate roundtrip to external authentication provider
    /// </summary>
    [HttpGet]
    public IActionResult Challenge(string scheme, string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
            returnUrl = "~/";

        // HACK: When a client implements OpenID Connect, we will uncomment this code and use the sign-in return url
        //// validate returnUrl - either it is a valid OIDC URL or back to a local page
        //if ((Url.IsLocalUrl(returnUrl) == false) && (_Interaction.IsValidReturnUrl(returnUrl) == false))

        //    // user might have clicked on a malicious link - should be logged
        //    throw new Exception("invalid return URL");

        // start challenge and roundtrip the return URL and scheme 
        AuthenticationProperties props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(Callback)),
            Items =
            {
                {"returnUrl", returnUrl},
                {"scheme", scheme}
            }
        };

        return Challenge(props, scheme);
    }

    /// <summary>
    ///     Post processing of external authentication
    /// </summary>
    /// <exception cref="Exception"></exception>
    [HttpGet]
    public async Task<IActionResult> Callback()
    {
        // read external identity from the temporary cookie
        AuthenticateResult result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        if (result?.Succeeded != true)
            throw new Exception("External authentication error");

        if (_Logger.IsEnabled(LogLevel.Debug))
        {
            IEnumerable<string> externalClaims = result.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}") ?? Array.Empty<string>();
            _Logger.LogDebug($"External claims: {externalClaims}");
        }

        // lookup our user and external provider info
        (IdentityUser? user, string provider, string providerUserId, IEnumerable<Claim> claims) = await FindUserFromExternalProvider(result);

        user ??= await AutoProvisionUser(provider, providerUserId, claims);

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for sign out from those protocols.
        List<Claim> additionalLocalClaims = new List<Claim>();
        AuthenticationProperties localSignInProps = new AuthenticationProperties();
        ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        IdentityServerUser issuer = new IdentityServerUser(user.Id)
        {
            DisplayName = user.UserName,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };

        await HttpContext.SignInAsync(issuer, localSignInProps);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // retrieve return URL
        string returnUrl = result.Properties?.Items["returnUrl"] ?? "~/";

        return Redirect(returnUrl);
    }

    private async Task<(IdentityUser? user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProvider(
        AuthenticateResult result)
    {
        ClaimsPrincipal externalUser = result.Principal;

        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used
        Claim userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject)
                            ?? externalUser.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("Unknown userid");

        // remove the user id claim so we don't include it as an extra claim if/when we provision the user
        List<Claim> claims = externalUser.Claims.ToList();
        claims.Remove(userIdClaim);

        string provider = result.Properties.Items["scheme"];
        string providerUserId = userIdClaim.Value;

        // find external user
        IdentityUser user = await _UserManager.FindByLoginAsync(provider, providerUserId);

        return (user, provider, providerUserId, claims);
    }

    private async Task<IdentityUser> AutoProvisionUser(string provider, string providerUserId, IEnumerable<Claim> claims)
    {
        // create dummy internal account (you can do something more complex)
        UserIdentity user = new UserIdentity(new UserDto()) {Id = Guid.NewGuid().ToString()};
        await _UserManager.CreateAsync(user);

        // add external user ID to new account
        await _UserManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

        return user;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        Claim sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));

        // if the external provider issued an id_token, we'll keep it for signout
        string idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
            localSignInProps.StoreTokens(new[]
            {
                new AuthenticationToken
                {
                    Name = "id_token",
                    Value = idToken
                }
            });
    }

    #endregion
}