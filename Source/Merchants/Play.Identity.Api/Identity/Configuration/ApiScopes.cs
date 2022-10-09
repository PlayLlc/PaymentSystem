using Duende.IdentityServer;

namespace Play.Identity.Api.Identity.Configuration;

public class ApiScopes
{
    public class IdentityServer
    {
        #region Static Metadata

        public const string Name = IdentityServerConstants.LocalApi.ScopeName;
        public const string Description = "This scope represents any client that is authorized to use an Identity Server resource";

        #endregion
    }

    public class ExternalMobile
    {
        #region Static Metadata

        public const string Name = nameof(ExternalMobile);
        public const string Description = "This scope represents clients calling from a mobile application";

        #endregion
    }

    public class ExternalApi
    {
        #region Static Metadata

        public const string Name = nameof(ExternalApi);
        public const string Description = "This scope represents clients calling from an external web api";

        #endregion
    }
}