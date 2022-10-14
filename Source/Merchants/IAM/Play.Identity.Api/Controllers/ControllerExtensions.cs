using Microsoft.AspNetCore.Mvc;

using Play.Identity.Api.Models;

namespace Play.Identity.Api.Controllers;

public static class ControllerExtensions
{
    #region Instance Members

    public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
    {
        controller.HttpContext.Response.StatusCode = 200;
        controller.HttpContext.Response.Headers["Location"] = "";

        return controller.View(viewName, new RedirectViewModel {RedirectUrl = redirectUri});
    }

    #endregion
}