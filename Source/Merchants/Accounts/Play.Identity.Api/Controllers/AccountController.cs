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

using System.Security.Policy;

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

        if (context is null)
        {
            await _InteractionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            return View("AccessDenied");
        }

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
        if ((User?.Identity?.IsAuthenticated ?? false) == false)
            return View("LoggedOut");

        LogoutRequest? context = await _InteractionService.GetLogoutContextAsync(logoutId);

        if (context.ClientId is null)
            return View("LoggedOut");

        if (context.ShowSignoutPrompt)
            return View(new LogoutViewModel() {LogoutId = logoutId});

        await HttpContext.SignOutAsync();

        if (context.PostLogoutRedirectUri is not null)
            Redirect(context.PostLogoutRedirectUri);

        return await Logout(new LogoutViewModel() {LogoutId = logoutId});
    }

    /// <summary>
    ///     Handles the response from the Logout confirmation page
    /// </summary>
    /// <exception cref="AggregateException"></exception>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutViewModel model)
    {
        if (TryGetExternalAuthenticateScheme(out string? externalScheme))
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Logout", new {logoutId = await _InteractionService.CreateLogoutContextAsync().ConfigureAwait(false)})
                }, externalScheme!);

        if (User?.Identity?.IsAuthenticated == true)
            await HttpContext.SignOutAsync();

        return View("LoggedOut");
    }

    /// <exception cref="AggregateException"></exception>
    private bool TryGetExternalAuthenticateScheme(out string? externalScheme)
    {
        externalScheme = null;
        string? idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

        if (idp is null)
            return false;

        if (idp == IdentityServerConstants.LocalIdentityProvider)
            return false;

        var supportsSignOutTask = HttpContext.GetSchemeSupportsSignOutAsync(idp);
        Task.WhenAll(supportsSignOutTask);

        if (!supportsSignOutTask.Result)
            return false;

        externalScheme = idp;

        return true;
    }

    #endregion
}