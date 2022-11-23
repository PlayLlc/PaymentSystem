using Duende.IdentityServer;

namespace Play.Identity.Api.Identity;

public static class IdentitySpecs
{
    public static class IdentityResources
    {
        #region Static Metadata

        public const string _OpenId = IdentityServerConstants.StandardScopes.OpenId;
        public const string _Phone = IdentityServerConstants.StandardScopes.Phone;
        public const string _Address = IdentityServerConstants.StandardScopes.Address;
        public const string _Email = IdentityServerConstants.StandardScopes.Email;

        #endregion
    }

    public class ApiScopes
    {
        #region Static Metadata

        public const string _OpenId = IdentityServerConstants.StandardScopes.OpenId;
        public const string _IdentityServer = IdentityServerConstants.LocalApi.ScopeName;

        public const string _ExternalMobile = nameof(_ExternalMobile);

        public const string _ExternalApi = nameof(_ExternalApi);
        public const string _Verification = "verification";

        #endregion
    }
}