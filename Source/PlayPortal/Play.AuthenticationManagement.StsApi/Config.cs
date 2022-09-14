using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Play.AuthenticationManagement.StsApi;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("merchantportal", "Merchant Portal", new List<string> { JwtClaimTypes.Name, JwtClaimTypes.Id, JwtClaimTypes.Subject, JwtClaimTypes.Email }),
            new ApiScope("iamapiportal", "Identity Authentication Management Portal")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "merchantportaluserclient",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                //secret for authentication
                ClientSecrets =
                {
                    new Secret("merchantportalsecret".Sha256())
                },

                //Scopes that the client has access to: this scope defines access to the MerchantPortalApi
                AllowedScopes = {
                    "merchantportal",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                }
            },
            new Client
            {
                ClientId = "identityServerLocalApi",

                AllowedGrantTypes = GrantTypes.Hybrid,

                ClientSecrets =
                {
                    new Secret("iamsecret".Sha256())
                },

                RequireClientSecret = false,
                //most likely to be used this way from the MerchantPortal API.
                AllowAccessTokensViaBrowser = true,
                //Scopes that the client has access to: this scope defines access to the Authentication APIs which are part of the IdentityServer for now.
                AllowedScopes = { "iamapiportal", "openid" }
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
                    new Claim(JwtClaimTypes.Name, "Test"),
                    new Claim(JwtClaimTypes.GivenName, "User"),
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
                    new Claim(JwtClaimTypes.Role, "Admin")
                }
            },
        };

}
