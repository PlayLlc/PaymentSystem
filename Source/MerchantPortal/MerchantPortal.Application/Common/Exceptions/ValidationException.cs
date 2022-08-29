using FluentValidation.Results;

namespace MerchantPortal.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation error occurred")
    {

    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
