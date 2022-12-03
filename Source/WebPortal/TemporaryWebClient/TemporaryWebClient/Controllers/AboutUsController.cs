using Microsoft.AspNetCore.Mvc;

using Play.Mvc.Extensions;

using TemporaryWebClient.Models;
using TemporaryWebClient.Services;

namespace TemporaryWebClient.Controllers;

public class AboutUsController : Controller
{
    #region Instance Values

    private readonly IEmailContactUsMessages _ContactUsEmailer;

    #endregion

    #region Constructor

    public AboutUsController(IEmailContactUsMessages contactUsEmailer)
    {
        _ContactUsEmailer = contactUsEmailer;
    }

    #endregion

    #region Instance Members

    [HttpPost]
    public async Task<IActionResult> ContactUs(ContactUsModel model)
    {
        this.ValidateModel();
        await _ContactUsEmailer.Send(model).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}