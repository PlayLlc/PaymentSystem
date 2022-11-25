using Duende.IdentityServer;

namespace Play.Identity.Api.Identity;

public static class IdentitySpecs
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