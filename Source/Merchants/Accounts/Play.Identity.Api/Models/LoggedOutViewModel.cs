// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace Play.Identity.Api.Models
{
    public class LoggedOutViewModel
    {
        #region Instance Values

        public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }

        public bool AutomaticRedirectAfterSignOut { get; set; }

        public string LogoutId { get; set; }
        public string ExternalAuthenticationScheme { get; set; }

        #endregion
    }
}