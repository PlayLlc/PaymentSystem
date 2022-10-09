using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using Duende.IdentityServer;

using IdentityModel;

using System.Security.Claims;
using System.Text.Json;

using _DeleteMe.Identity.Enums;

namespace _DeleteMe.Configuration
{
    public static class IdentityConfig
    {
        #region Instance Members

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new()
                {
                    Name = JwtClaimTypes.Role,
                    DisplayName = JwtClaimTypes.Role,
                    UserClaims = {JwtClaimTypes.Role}
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new(ApiScopes.IdentityServer.Name, ApiScopes.IdentityServer.Description, new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role
                }),
                new(ApiScopes.ExternalApi.Name, ApiScopes.ExternalApi.Description, new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role
                }),
                new(ApiScopes.ExternalMobile.Name, ApiScopes.ExternalMobile.Description, new List<string>
                {
                    JwtClaimTypes.Id,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.Address,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role
                })
            };
        }

        public static List<TestUser> GetTestUsers(ConfigurationManager configurationManager)
        {
            TestUserConfig? testUserConfig = configurationManager.GetSection(nameof(TestUserConfig)).Get<TestUserConfig>();
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
                    Username = testUserConfig.Email,
                    Password = testUserConfig.Password,
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Subject, "1903ac50-6f92-4759-bf08-2819ff84cf76"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, testUserConfig.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
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
                    ClientId = merchantPortalConfig.ClientId,
                    ClientName = merchantPortalConfig.ClientName,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new(merchantPortalConfig.ClientSecret.Sha256())},
                    RedirectUris = merchantPortalConfig.RedirectUris.ToArray(),
                    AllowedScopes =
                    {
                        ApiScopes.IdentityServer.Name,
                        ApiScopes.ExternalApi.Name
                    }
                },
                new()
                {
                    ClientId = businessPayConfig.ClientId,
                    ClientName = businessPayConfig.ClientName,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new(businessPayConfig.ClientSecret.Sha256())},
                    AllowedScopes =
                    {
                        ApiScopes.IdentityServer.Name,
                        ApiScopes.ExternalMobile.Name
                    }
                }
            };
        }

        #endregion
    }
}