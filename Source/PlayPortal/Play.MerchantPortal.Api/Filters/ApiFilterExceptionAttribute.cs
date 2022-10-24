﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Play.MerchantPortal.Application.Common.Exceptions;

namespace Play.MerchantPortal.Api.Filters;

public class ApiFilterExceptionAttribute : ExceptionFilterAttribute
{
    #region Instance Values

    private readonly IDictionary<Type, Action<ExceptionContext>> _ExceptionHandlers;

    #endregion

    #region Constructor

    public ApiFilterExceptionAttribute()
    {
        _ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>()
        {
            {typeof(NotFoundException), HandleNotFoundException}, {typeof(ModelValidationException), HandleValidationException}
        };
    }

    #endregion

    #region Instance Members

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();

        if (_ExceptionHandlers.ContainsKey(type))
        {
            _ExceptionHandlers[type].Invoke(context);

            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);

            return;
        }
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState) {Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"};

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException) context.Exception;

        var details = new HttpValidationProblemDetails() {Title = "The specified resource was not found.", Detail = exception.Message};

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ModelValidationException) context.Exception;

        var problemDetails = new ValidationProblemDetails(exception.Errors) {Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1"};

        context.Result = new BadRequestObjectResult(problemDetails);

        context.ExceptionHandled = true;
    }

    #endregion
}