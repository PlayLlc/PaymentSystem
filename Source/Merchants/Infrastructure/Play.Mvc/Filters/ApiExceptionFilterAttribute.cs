using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Play.Core.Exceptions;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Mvc.Exceptions;
using Play.Restful.Clients;

namespace Play.Mvc.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    #region Instance Values

    private readonly ILogger<ApiExceptionFilterAttribute> _Logger;
    private readonly IDictionary<Type, Action<ExceptionContext>> _ExceptionHandlers;

    #endregion

    #region Constructor

    public ApiExceptionFilterAttribute()
    {
        LoggerFactory a = new LoggerFactory();

        _Logger = a.CreateLogger<ApiExceptionFilterAttribute>();
        _ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {typeof(ModelValidationException), HandleModelValidationException},
            {typeof(BusinessRuleValidationException), HandleBusinessRuleValidationException},
            {typeof(ValueObjectException), HandleValueObjectException},
            {typeof(RepositoryException), HandleRepositoryException},
            {typeof(AggregateException), HandleAggregateException},
            {typeof(NotFoundException), HandleNotFoundException},
            {typeof(CommandOutOfSyncException), HandleCommandOutOfSyncException},
            {typeof(PlayInternalException), HandlePlayInternalException},
            {typeof(InvalidOperationException), HandleInvalidOperationExceptionException},
            {typeof(Exception), HandleUncaughtException}
        };
    }

    #endregion

    #region Instance Members

    private void HandleApiException(ExceptionContext context)
    {
        ApiException exception = (ApiException) context.Exception;

        ProblemDetails details = new ProblemDetails
        {
            Title = "An unhandled exception was thrown while using a Restful API Client",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleInvalidOperationExceptionException(ExceptionContext context)
    {
        InvalidOperationException exception = (InvalidOperationException) context.Exception;

        ValidationProblemDetails details = new ValidationProblemDetails(context.ModelState)
        {
            Title = "An invalid operation was attempted resulting in an Internal Server Error",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleModelValidationException(ExceptionContext context)
    {
        ModelValidationException exception = (ModelValidationException) context.Exception;

        ValidationProblemDetails details = new ValidationProblemDetails(context.ModelState)
        {
            Title = "A bad request caused Model validation to fail",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandlePlayInternalException(ExceptionContext context)
    {
        PlayInternalException exception = (PlayInternalException) context.Exception;

        ProblemDetails details = new ProblemDetails
        {
            Title = "An uncaught internal exception has caused an Internal Server Error",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        NotFoundException exception = (NotFoundException) context.Exception;

        ProblemDetails details = new ProblemDetails
        {
            Title = "The requested resource could not be found",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.NotFound
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleAggregateException(ExceptionContext context)
    {
        AggregateException exception = (AggregateException) context.Exception;

        ProblemDetails details = new ProblemDetails
        {
            Title = "An uncaught exception likely due to an incorrect asynchronous implementation has caused an Internal Server Error",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleUncaughtException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        ProblemDetails details = new ProblemDetails
        {
            Title = "An uncaught exception caused an Internal Server Error",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.InternalServerError
        };
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;

        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleCommandOutOfSyncException(ExceptionContext context)
    {
        CommandOutOfSyncException exception = (CommandOutOfSyncException) context.Exception;

        HttpValidationProblemDetails details = new HttpValidationProblemDetails
        {
            Title = "The request could not be completed because of an invalid state. A resource was expected to exist but could not be found",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.NotFound
        };
        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleRepositoryException(ExceptionContext context)
    {
        RepositoryException exception = (RepositoryException) context.Exception;

        HttpValidationProblemDetails details = new HttpValidationProblemDetails
        {
            Title = "A problem occurred locating a resource required to complete the request",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.NotFound
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleBusinessRuleValidationException(ExceptionContext context)
    {
        BusinessRuleValidationException exception = (BusinessRuleValidationException) context.Exception;

        HttpValidationProblemDetails details = new HttpValidationProblemDetails
        {
            Title = "The request could not properly be processed because a business rule has been violated.",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.BadRequest
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    private void HandleValueObjectException(ExceptionContext context)
    {
        ValueObjectException exception = (ValueObjectException) context.Exception;
        HttpValidationProblemDetails details = new HttpValidationProblemDetails
        {
            Title = "There was a problem with the format of the message sent.",
            Detail = exception.Message,
            Status = (int) HttpStatusCode.BadRequest
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
        _Logger.Log(LogLevel.Error, details.Detail, details);
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
        _Logger.Log(LogLevel.Error, details.Detail, details);
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        //base.OnException(context);
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
            HandleInvalidModelStateException(context);
    }

    #endregion

    //InvalidOperationException
}