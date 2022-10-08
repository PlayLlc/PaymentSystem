using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using Duende.IdentityServer;

using IdentityModel;

using System.Security.Claims;
using System.Text.Json;

namespace _DeleteMe.Configuration
{
    public static class IdentityConfig
    {
        #region Instance Values

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource> {new IdentityResources.OpenId(), new IdentityResources.Profile()};

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new(nameof(_DeleteMe), "User Identity Management",
                    new List<string> {JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.Email, JwtClaimTypes.PhoneNumber})
            };

        #endregion

        #region Instance Members

        public static List<TestUser> GetTestUsers(ConfigurationManager configurationManager)
        {
            var testUserConfig = configurationManager.GetSection(nameof(TestUserConfig)).Get<TestUserConfig>();
            var address = new {street_address = "One Hacker Way", locality = "Dallas", postal_code = 75036, country = "United States"};

            return new List<TestUser>()
            {
                new()
                {
                    SubjectId = "1", Username = testUserConfig.Email, Password = testUserConfig.Password,
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"), new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"), new Claim(JwtClaimTypes.Email, testUserConfig.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClientConfiguration(ConfigurationManager configurationManager)
        {
            var merchantPortalConfig = configurationManager.GetSection(nameof(MerchantPortalConfig)).Get<MerchantPortalConfig>();
            var businessPayConfig = configurationManager.GetSection(nameof(BusinessPayConfig)).Get<BusinessPayConfig>();

            return new List<Client>
            {
                new()
                {
                    ClientId = merchantPortalConfig.ClientId, ClientName = merchantPortalConfig.ClientName,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new(merchantPortalConfig.ClientSecret.Sha256())},
                    RedirectUris = merchantPortalConfig.RedirectUris.ToArray(),
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, merchantPortalConfig.ClientId
                    }
                },
                new()
                {
                    ClientId = businessPayConfig.ClientId, ClientName = businessPayConfig.ClientName, AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new(businessPayConfig.ClientSecret.Sha256())},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, businessPayConfig.ClientId
                    }
                }
            };
        }

        #endregion
    }
}