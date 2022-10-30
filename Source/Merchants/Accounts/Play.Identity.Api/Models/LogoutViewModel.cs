// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace Play.Identity.Api.Models
{
    public class LogoutViewModel : LogoutInputModel
    {
        #region Instance Values

        public bool ShowLogoutPrompt { get; set; } = true;

        #endregion
    }
}