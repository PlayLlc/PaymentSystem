using Microsoft.AspNetCore.Mvc;

using Play.Mvc.Exceptions;

namespace Play.Mvc.Extensions;

public static class ControllerExtensions
{
    #region Instance Members

    /// <exception cref="ModelValidationException"></exception>
    public static void ValidateModel(this Controller controller)
    {
        if (!controller.ModelState.IsValid)
            throw new ModelValidationException(
                $"Model validation failed due to a bad request. Errors: [{controller?.ModelState?.Values.SelectMany(v => v.Errors)?.Select(e => e.ErrorMessage) ?? Array.Empty<string>()}]");
    }

    public static void ValidateModel(this ControllerBase controller)
    {
        if (!controller.ModelState.IsValid)
            throw new ModelValidationException(
                $"Model validation failed due to a bad request. Errors: [{controller?.ModelState?.Values.SelectMany(v => v.Errors)?.Select(e => e.ErrorMessage) ?? Array.Empty<string>()}]");
    }

    #endregion
}