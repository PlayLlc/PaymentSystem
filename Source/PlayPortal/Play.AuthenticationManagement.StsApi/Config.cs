using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Play.AuthenticationManagement.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("merchantportal", "Merchant Portal", new List<string> { JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.Email }),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "merchantportal_client",
                ClientName = "Merchant Portal Client",
                AllowedGrantTypes = GrantTypes.Code,

                //secret for authentication
                ClientSecrets =
                {
                    new Secret("cb17c97c-0910-41c0-aafb-2b77a5838852".Sha256())
                },
                RedirectUris = { "https://localhost:7133/signin-oidc" },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowOfflineAccess = true,

                //Scopes that the client has access to: this scope defines access to the MerchantPortalApi
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "merchantportal",
                }
            }
        };

    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "subjectId1",
                Username = "merchantuser",
                Password = "merchantuser",
                IsActive = true,
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, "Test2"),
                    new Claim(JwtClaimTypes.GivenName, "User2"),
                    new Claim(JwtClaimTypes.Role, "Merchant")
                }
            },
            new TestUser
            {
                SubjectId = "subjectId2",
                Username = "admin",
                Password = "admin",
                IsActive = true,
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Name, "Test"),
                    new Claim(JwtClaimTypes.GivenName, "User"),
                    new Claim(JwtClaimTypes.Role, "Admin"),
                    new Claim(JwtClaimTypes.Email, "testemail@notused.com")
                }
            },
        };
}
