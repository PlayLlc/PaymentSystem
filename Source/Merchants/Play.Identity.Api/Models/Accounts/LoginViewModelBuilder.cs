using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;

using Microsoft.AspNetCore.Authentication;

using NuGet.Packaging;

namespace Play.Identity.Api.Models.Accounts;

public class LoginViewModelBuilder : IBuildLoginViewModel
{
    #region Instance Values

    private readonly IIdentityServerInteractionService _InteractionService;
    private readonly IAuthenticationSchemeProvider _SchemeProvider;
    private readonly IIdentityProviderStore _IdentityProviderStore;
    private readonly IClientStore _ClientStore;

    #endregion

    #region Constructor

    public LoginViewModelBuilder(
        IIdentityServerInteractionService interactionService, IAuthenticationSchemeProvider schemeProvider, IIdentityProviderStore identityProviderStore,
        IClientStore clientStore)
    {
        _InteractionService = interactionService;
        _SchemeProvider = schemeProvider;
        _IdentityProviderStore = identityProviderStore;
        _ClientStore = clientStore;
    }

    #endregion

    #region Instance Members

    public async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        AuthorizationRequest context = await _InteractionService.GetAuthorizationContextAsync(returnUrl);

        var modelFromScheme = await TryGetLoginViewModelFromScheme(context, returnUrl).ConfigureAwait(false);

        if (modelFromScheme is not null)
            return modelFromScheme;

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            ReturnUrl = returnUrl,
            Username = context?.LoginHint,
            ExternalProviders = await GetExternalProvidersAsync(context).ConfigureAwait(false)
        };
    }

    private async Task<LoginViewModel?> TryGetLoginViewModelFromScheme(AuthorizationRequest context, string returnUrl)
    {
        if (context?.IdP == null)
            return null;

        if (await _SchemeProvider.GetSchemeAsync(context.IdP) != null)
            return null;

        LoginViewModel vm = new LoginViewModel
        {
            ReturnUrl = returnUrl,
            Username = context?.LoginHint ?? string.Empty
        };

        vm.ExternalProviders = new[] {new ExternalProvider {AuthenticationScheme = context?.IdP ?? string.Empty}};

        return vm;
    }

    private async Task<HashSet<ExternalProvider>> GetExternalProvidersAsync(AuthorizationRequest context)
    {
        IEnumerable<AuthenticationScheme> schemes = await _SchemeProvider.GetAllSchemesAsync();

        HashSet<ExternalProvider> providers = schemes.Where(x => x.DisplayName != null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            })
            .ToHashSet();

        IEnumerable<ExternalProvider> identityProvidersSchemes = (await _IdentityProviderStore.GetAllSchemeNamesAsync())
            .Where(x => x.Enabled && !string.IsNullOrWhiteSpace(x.DisplayName))
            .Select(x => new ExternalProvider
            {
                AuthenticationScheme = x.Scheme,
                DisplayName = x.DisplayName
            });

        providers.AddRange(identityProvidersSchemes);

        if (context?.Client.ClientId is null)
            return providers;

        Client client = await _ClientStore.FindEnabledClientByIdAsync(context.Client.ClientId).ConfigureAwait(false);

        if (client is null)
            return providers;

        if (client.IdentityProviderRestrictions is null)
            return providers;

        if (!client.IdentityProviderRestrictions.Any())
            return providers;

        return providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToHashSet();
    }

    #endregion
}