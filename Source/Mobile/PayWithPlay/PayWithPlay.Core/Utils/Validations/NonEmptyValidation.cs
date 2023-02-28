using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Utils.Validations
{
    public class NonEmptyValidation : BaseValidation
    {
        public NonEmptyValidation(string errorMessage = "") : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorMessage = Resource.InvalidEmptyField;
            }
        }

        public override ValidationResult GetValidationResult(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? ValidationResultFactory.Error(ErrorMessage) : ValidationResultFactory.Success;
        }
    }
}