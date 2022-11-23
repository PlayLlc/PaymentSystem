using Duende.IdentityServer;
using Duende.IdentityServer.Models;

using IdentityModel;

namespace Play.Identity.Api.Identity;

public static class IdentityInMemoryConfig
{
    #region Instance Members

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Phone(),
            new IdentityResources.Address(),
            new IdentityResources.Email()
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new(IdentitySpecs.ApiScopes._IdentityServer, "This scope represents any client that is authorized to use an Identity Server resource",
                new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email
                }),
            new(IdentitySpecs.ApiScopes._ExternalApi, "This scope represents clients calling from an external web api", new List<string>
            {
                JwtClaimTypes.Id,
                JwtClaimTypes.ClientId,
                JwtClaimTypes.PhoneNumber,
                JwtClaimTypes.Address,
                JwtClaimTypes.Email
            }),
            new(IdentitySpecs.ApiScopes._ExternalMobile, "This scope represents clients calling from a mobile application",
                new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email
                }),
            new(IdentitySpecs.ApiScopes._Verification, "This scope allows clients to see if the user's account information has been verified",
                new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.PhoneNumberVerified,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                })
        };
    }

    public static IEnumerable<Client> GetClients(ConfigurationManager configurationManager)
    {
        MerchantPortalClient? merchantPortalConfig = configurationManager.GetSection(nameof(MerchantPortalClient)).Get<MerchantPortalClient>();
        BusinessPayClient? businessPayConfig = configurationManager.GetSection(nameof(BusinessPayClient)).Get<BusinessPayClient>();

        return new List<Client>
        {
            new()
            {
                ClientId = businessPayConfig.ClientId,
                ClientName = businessPayConfig.ClientName,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> {new(businessPayConfig.ClientSecret.Sha256())},
                RedirectUris = {businessPayConfig.RedirectUris},
                PostLogoutRedirectUris = {businessPayConfig.PostLogoutRedirectUris},
                AllowedScopes =
                {
                    IdentitySpecs.ApiScopes._IdentityServer,
                    IdentitySpecs.ApiScopes._ExternalMobile,
                    IdentitySpecs.ApiScopes._OpenId
                }
            },

            // interactive client such as web applications, SPAs or native/mobile apps with interactive users who interact
            // with a browser page for login, consent, etc
            new()
            {
                ClientId = merchantPortalConfig.ClientId,
                ClientName = merchantPortalConfig.ClientName,
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = new List<Secret> {new(merchantPortalConfig.ClientSecret.Sha256())},
                RedirectUris = {merchantPortalConfig.RedirectUris},
                PostLogoutRedirectUris = {merchantPortalConfig.PostLogoutRedirectUris},
                AllowedScopes =
                {
                    IdentitySpecs.ApiScopes._OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentitySpecs.ApiScopes._IdentityServer,
                    IdentitySpecs.ApiScopes._ExternalApi
                }
            }
        };
    }

    #endregion
}