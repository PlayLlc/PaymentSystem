using Microsoft.AspNetCore.Mvc;

using Play.Registration.Application.Services.Emails;

namespace Play.Registration.Api.Services;

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

    public string CreateReturnUrl(string email, uint confirmationCode)
    {
        string scheme = _UrlHelper.ActionContext.HttpContext.Request.Scheme;
        var aa = _UrlHelper.Action("EmailVerification", "User", new
        {
            email = email,
            confirmationCode = confirmationCode
        }, scheme);

        //var a = _UrlHelper.RouteUrl("Registration/User/EmailVerification", new VerifyConfirmationCodeCommand
        //{
        //    ConfirmationCode = confirmationCode,
        //    UserRegistrationId = userRegistrationId
        //});

        return aa;
    }

    #endregion
}