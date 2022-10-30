using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Accounts.Persistence.Sql.Entities;
using Play.Accounts.Persistence.Sql.Repositories;
using Play.Core;
using Play.Domain.Exceptions;
using Play.Identity.Api.Models;
using Play.Identity.Api.Services;
using Play.Mvc.Attributes;

namespace Play.Identity.Api.Controllers;

/// <summary>
///     Provides OpenID Connect and OAuth 2.0 Authentication and Authorization to client applications
/// </summary>
[SecurityHeaders]
[AllowAnonymous]
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    #region Instance Values

    private readonly IIdentityServerInteractionService _InteractionService;
    private readonly IUserRepository _UserRepository;
    private readonly ILoginUsers _UserLoginService;
    private readonly IAuthenticationSchemeProvider _SchemeProvider; // in memory? 
    private readonly IIdentityProviderStore _IdentityProviderStore; // persistence?
    private readonly IClientStore _ClientStore;

    #endregion

    #region Constructor

    /// <inheritdoc />
    public AccountController(
        IIdentityServerInteractionService interactionService, IUserRepository userRepository, ILoginUsers userLoginService,
        IAuthenticationSchemeProvider schemeProvider, IIdentityProviderStore identityProviderStore, IClientStore clientStore)
    {
        _InteractionService = interactionService;
        _UserRepository = userRepository;
        _UserLoginService = userLoginService;
        _SchemeProvider = schemeProvider;
        _IdentityProviderStore = identityProviderStore;
        _ClientStore = clientStore;
    }

    #endregion

    #region Instance Members

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #endregion

    #region Login

    /// <summary>
    ///     Returns a view that allows the user to sign into the build int Identity Authentication Server or an External Open
    ///     ID Connect Identity Provider
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        LoginViewModel vm = await BuildLoginViewModelAsync(returnUrl).ConfigureAwait(false);

        return View(vm);
    }

    /// <summary>
    ///     Attempts to log the user into the built in Identity Authentication Server or an External Open ID Connect Identity
    ///     Provider
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginViewModel model)
    {
        AuthorizationRequest? context = await _InteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

        // if we don't have a valid context then we will reload the login page
        if (context is null)
            return await Login(model.ReturnUrl);

        if (!ModelState.IsValid)
            return View(await BuildLoginViewModelAsync(model).ConfigureAwait(false));

        User user = await _UserRepository.GetByEmailAsync(model.Username).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Result loginResult = await _UserLoginService.LoginAsync(HttpContext, user, model.Password).ConfigureAwait(false);

        if (!loginResult.Succeeded)
        {
            foreach (string error in loginResult.Errors)
                ModelState.AddModelError(string.Empty, error);

            return View(await BuildLoginViewModelAsync(model).ConfigureAwait(false));
        }

        return Redirect(model.ReturnUrl);
    }

    #region Build Login Model

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
    {
        LoginViewModel vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Username = model.Username;

        return vm;
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        AuthorizationRequest context = await _InteractionService.GetAuthorizationContextAsync(returnUrl);

        return new LoginViewModel
        {
            ReturnUrl = returnUrl,
            Username = context?.LoginHint ?? string.Empty,
            ExternalProviders = await GetExternalProvidersAsync(context!).ConfigureAwait(false)
        };
    }

    private async Task<IEnumerable<ExternalProviderModel>> GetExternalProvidersAsync(AuthorizationRequest authorizationRequest)
    {
        List<ExternalProviderModel> providers = new();

        providers.AddRange((await _SchemeProvider.GetAllSchemesAsync().ConfigureAwait(false)).Where(x => x.DisplayName != null)
            .Select(x => new ExternalProviderModel
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            })
            .ToList());

        providers.AddRange((await _IdentityProviderStore.GetAllSchemeNamesAsync()).Where(x => x.Enabled && !string.IsNullOrWhiteSpace(x.DisplayName))
            .Select(x => new ExternalProviderModel
            {
                AuthenticationScheme = x.Scheme,
                DisplayName = x.DisplayName
            }));

        if (authorizationRequest?.Client.ClientId is null)
            return providers;

        Client client = await _ClientStore.FindEnabledClientByIdAsync(authorizationRequest.Client.ClientId).ConfigureAwait(false);

        if (client is null)
            return providers;

        if (client.IdentityProviderRestrictions is null)
            return providers;

        if (!client.IdentityProviderRestrictions.Any())
            return providers;

        return providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
    }

    #endregion

    #endregion

    #region Logout

    /// <summary>
    ///     Show logout page
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        // build a model so the logout page knows what to display
        var vm = await BuildLogoutViewModelAsync(logoutId);

        if (vm.ShowLogoutPrompt == false)

            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await Logout(vm);

        return View(vm);
    }

    /// <summary>
    ///     Handle logout page postback
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        // build a model so the logged out page knows what to display
        LoggedOutViewModel vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        if (User?.Identity.IsAuthenticated == true)

            // delete local authentication cookie
            await HttpContext.SignOutAsync();

        //// raise the logout event
        //await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

        // check if we need to trigger sign-out at an upstream identity provider
        if (vm.TriggerExternalSignout)
        {
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.
            string url = Url.Action("Logout", new {logoutId = vm.LogoutId});

            // this triggers a redirect to the external provider for sign-out
            return SignOut(new AuthenticationProperties {RedirectUri = url}, vm.ExternalAuthenticationScheme);
        }

        return View("LoggedOut", vm);
    }

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        LogoutRequest? logout = await _InteractionService.GetLogoutContextAsync(logoutId);

        LoggedOutViewModel vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (User?.Identity.IsAuthenticated == true)
        {
            string? idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if ((idp != null) && (idp != IdentityServerConstants.LocalIdentityProvider))
            {
                bool providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);

                if (providerSupportsSignout)
                {
                    if (vm.LogoutId == null)

                        // if there's no current logout context, we need to create one
                        // this captures necessary info from the current logged in user
                        // before we signout and redirect away to the external IdP for signout
                        vm.LogoutId = await _InteractionService.CreateLogoutContextAsync();

                    vm.ExternalAuthenticationScheme = idp;
                }
            }
        }

        return vm;
    }

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
    {
        LogoutViewModel vm = new LogoutViewModel
        {
            LogoutId = logoutId,
            ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt
        };

        if (User?.Identity.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            vm.ShowLogoutPrompt = false;

            return vm;
        }

        LogoutRequest? context = await _InteractionService.GetLogoutContextAsync(logoutId);

        if (context?.ShowSignoutPrompt == false)
        {
            // it's safe to automatically sign-out
            vm.ShowLogoutPrompt = false;

            return vm;
        }

        // show the logout prompt. this prevents attacks where the user
        // is automatically signed out by another malicious web page.
        return vm;
    }

    #endregion
}