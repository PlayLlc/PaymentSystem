using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;

using Microsoft.AspNetCore.Authentication;

using Play.Identity.Api.Models;

namespace Play.Identity.Api;

public class LoginViewModelBuilder : IBuildLoginViewModel
{
    #region Instance Values

    private readonly IIdentityServerInteractionService _InteractionService;

    // in memory
    private readonly IAuthenticationSchemeProvider _SchemeProvider;

    // persistence
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

    public async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
    {
        LoginViewModel vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Username = model.Username;

        return vm;
    }

    public async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
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
}