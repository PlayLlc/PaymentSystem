using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Application.Services;
using Play.Accounts.Contracts.Commands;

namespace Play.Identity.Api.Services;

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