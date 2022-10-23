using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Play.Accounts.Application.Services.Emails;
using Play.Accounts.Contracts.Commands;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Play.Identity.Api._TempServices
{
    public class EmailVerificationReturnUrlGenerator : ICreateEmailVerificationReturnUrl
    {
        #region Instance Values

        private readonly IUrlHelper _UrlHelper;

        #endregion

        #region Constructor

        public EmailVerificationReturnUrlGenerator(IUrlHelper urlHelper)
        {
            _UrlHelper = urlHelper;
        }

        #endregion

        #region Instance Members

        public string CreateReturnUrl(string userRegistrationId, uint confirmationCode)
        {
            return _UrlHelper.Action("EmailVerification", "User", new VerifyConfirmationCodeCommand()
                   {
                       ConfirmationCode = confirmationCode,
                       UserRegistrationId = userRegistrationId
                   })
                   ?? throw new InvalidOleVariantTypeException();
        }

        #endregion
    }
}