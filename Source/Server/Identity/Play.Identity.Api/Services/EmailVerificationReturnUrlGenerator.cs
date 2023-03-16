using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Mvc;

using Play.Identity.Application.Services;
using Play.Identity.Contracts.Commands;

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
        var aa = _UrlHelper.Action("EmailVerification", "User", new
        {
            area = "Registration",
            userRegistrationId = userRegistrationId,
            confirmationCode = confirmationCode
        });

        //var a = _UrlHelper.RouteUrl("Registration/User/EmailVerification", new VerifyConfirmationCodeCommand
        //{
        //    ConfirmationCode = confirmationCode,
        //    UserRegistrationId = userRegistrationId
        //});

        return aa;
    }

    #endregion
}