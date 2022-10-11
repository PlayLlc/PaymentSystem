using System.Security.Claims;
using System.Text.Json;

using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

using IdentityModel;

namespace Play.Identity.Api.Identity.Configuration
{
    public static class Specs
    {
        public static class IdentityResources
        {
            #region Static Metadata

            public const string OpenId = IdentityServerConstants.StandardScopes.OpenId;
            public const string Phone = IdentityServerConstants.StandardScopes.Phone;
            public const string Address = IdentityServerConstants.StandardScopes.Address;
            public const string Email = IdentityServerConstants.StandardScopes.Email;

            #endregion
        }

        public class ApiScopes
        {
            #region Static Metadata

            public const string OpenId = IdentityServerConstants.StandardScopes.OpenId;
            public const string IdentityServer = IdentityServerConstants.LocalApi.ScopeName;

            public const string ExternalMobile = nameof(ExternalMobile);

            public const string ExternalApi = nameof(ExternalApi);
            public const string Verification = "verification";

            #endregion
        }
    }

    public static class IdentityConfig
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
                new(Specs.ApiScopes.IdentityServer, "This scope represents any client that is authorized to use an Identity Server resource",
                    new List<string>
                    {
                        JwtClaimTypes.Id,
                        JwtClaimTypes.ClientId,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Address,
                        JwtClaimTypes.Email
                    }),
                new(Specs.ApiScopes.ExternalApi, "This scope represents clients calling from an external web api", new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email
                }),
                new(Specs.ApiScopes.ExternalMobile, "This scope represents clients calling from a mobile application", new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email
                }),
                new(Specs.ApiScopes.Verification, "This scope allows clients to see if the user's account information has been verified",
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
                        Specs.ApiScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile

                        //Specs.ApiScopes.IdentityServer,
                        //Specs.ApiScopes.ExternalApi
                    }
                },
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
                        Specs.ApiScopes.IdentityServer,
                        Specs.ApiScopes.ExternalMobile,
                        Specs.ApiScopes.OpenId
                    }
                }
            };
        }

        public static List<TestUser> GetTestUsers(ConfigurationManager configurationManager)
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Dallas",
                postal_code = 75036,
                country = "United States"
            };

            return new List<TestUser>()
            {
                new()
                {
                    SubjectId = "1",
                    Username = "test",
                    Password = "test",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Ralph Nader"),
                        new Claim(JwtClaimTypes.GivenName, "Ralph"),
                        new Claim(JwtClaimTypes.FamilyName, "Nader"),
                        new Claim(JwtClaimTypes.Email, "enron@aol.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }

        #endregion
    }
}