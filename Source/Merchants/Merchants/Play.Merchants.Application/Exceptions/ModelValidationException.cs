using FluentValidation.Results;

namespace Play.Merchants.Application.Exceptions;

public class ModelValidationException : Exception
{
    #region Instance Values

    public IDictionary<string, string[]> Errors { get; }

    #endregion

    #region Constructor

    public ModelValidationException(IEnumerable<ValidationFailure> failures) : base("One or more validation error occurred")
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    #endregion
}