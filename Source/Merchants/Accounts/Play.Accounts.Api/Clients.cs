using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace Play.Accounts.Api
{
    internal class Clients
    {
        #region Instance Members

        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "oauthClient", ClientName = "Example client application using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, ClientSecrets = new List<Secret> {new("SuperSecretPassword".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"api1.read"}
                },
                new()
                {
                    ClientId = "oidcClient", ClientName = "Example Client Application",
                    ClientSecrets = new List<Secret> {new("SuperSecretPassword".Sha256())}, // change me!

                    AllowedGrantTypes = GrantTypes.Code, RedirectUris = new List<string> {"https://localhost:5002/signin-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email, "role", "api1.read"
                    },
                    RequirePkce = true, AllowPlainTextPkce = false
                }
            };
        }

        #endregion
    }
}