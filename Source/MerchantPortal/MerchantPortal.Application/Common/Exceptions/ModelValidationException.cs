using FluentValidation.Results;

namespace MerchantPortal.Application.Common.Exceptions;

public class ModelValidationException : Exception
{
    public ModelValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation error occurred")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
