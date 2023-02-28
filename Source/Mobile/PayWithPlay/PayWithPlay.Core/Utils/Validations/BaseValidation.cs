using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Utils.Validations
{
    public abstract class BaseValidation : IValidation
    {
        public BaseValidation(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string? ErrorMessage { get; set; }

        public string EmptyErrorMessage { get; set; } = Resource.InvalidEmptyField;

        public bool IsOptional { get; set; }

        public abstract ValidationResult GetValidationResult(string value);
    }
}