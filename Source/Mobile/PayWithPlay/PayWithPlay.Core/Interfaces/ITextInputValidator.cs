using PayWithPlay.Core.Utils.Validations;

namespace PayWithPlay.Core.Interfaces
{
    public interface ITextInputValidator
    {
        ValidationResult GetValidationResult();

        ValidationResult PerformValidation(bool displayError, bool hasFocus);

        IList<IValidation>? Validations { get; set; }

        string? Value { get; }
    }
}