using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Play.Domain;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;

namespace Play.Identity.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    #region Instance Values

    private readonly IDictionary<Type, Action<ExceptionContext>> _ExceptionHandlers;

    #endregion

    #region Constructor

    public ApiExceptionFilterAttribute()
    {
        _ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>()
        {
            {typeof(ValueObjectException), HandleValueObjectException},
            {typeof(BusinessRuleValidationException), HandleValueObjectException},
            {typeof(RepositoryException), HandleRepositoryException},
            {typeof(CommandOutOfSyncException), HandleCommandOutOfSyncException},
            {typeof(Exception), HandleUncaughtException}
        };
    }

    #endregion

    #region Instance Members

    private void HandleUncaughtException(ExceptionContext context)
    {
        Exception exception = (Exception) context.Exception;

        ProblemDetails details = new ProblemDetails()
        {
            Title = "An uncaught exception caused an Internal Server Error",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleCommandOutOfSyncException(ExceptionContext context)
    {
        CommandOutOfSyncException exception = (CommandOutOfSyncException) context.Exception;

        HttpValidationProblemDetails details = new HttpValidationProblemDetails()
        {
            Title = "The request could not be completed because of an invalid state. A resource was expected to exist but could not be found",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.NotFound
        };
        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleRepositoryException(ExceptionContext context)
    {
        RepositoryException exception = (RepositoryException) context.Exception;

        HttpValidationProblemDetails details = new HttpValidationProblemDetails()
        {
            Title = "A problem occurred locating a resource required to complete the request",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.NotFound
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleBusinessRuleValidationException(ExceptionContext context)
    {
        BusinessRuleValidationException exception = (BusinessRuleValidationException) context.Exception;

        HttpValidationProblemDetails details = new HttpValidationProblemDetails()
        {
            Title = "The request could not properly be processed because a business rule has been violated.",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.BadRequest
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleValueObjectException(ExceptionContext context)
    {
        ValueObjectException exception = (ValueObjectException) context.Exception;
        HttpValidationProblemDetails details = new HttpValidationProblemDetails()
        {
            Title = "There was a problem with the format of the message sent.",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.BadRequest
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    /// <exception cref="OutOfMemoryException"></exception>
    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        IEnumerable<string> modelStateErrors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        ValidationProblemDetails details = new ValidationProblemDetails(context.ModelState)
        {
            Title = "There was a problem with the format of the request message",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Detail = string.Join(" | ", modelStateErrors)
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

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

    #endregion
}