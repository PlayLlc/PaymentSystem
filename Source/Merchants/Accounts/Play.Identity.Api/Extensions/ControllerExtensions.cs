using Microsoft.AspNetCore.Mvc;

using SendGrid.Helpers.Errors.Model;

namespace Play.Identity.Api.Extensions;

public static class ControllerExtensions
{
    #region Instance Members

    /// <exception cref="BadRequestException"></exception>
    internal static void ValidateModel(this Controller controller)
    {
        if (!controller.ModelState.IsValid)
            throw new BadRequestException(
                $"Model validation failed due to a bad request. Errors: [{controller?.ModelState?.Values.SelectMany(v => v.Errors)?.Select(e => e.ErrorMessage) ?? Array.Empty<string>()}]");
    }

    #endregion
}