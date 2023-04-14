using PayWithPlay.Core.Extensions;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Utils.Validations
{
    public class MinLengthValidation : BaseValidation
    {
        private int _minValue;

        public MinLengthValidation(int minValue = -1, string errorMessage = "") : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage) && Resource.InvalidMinLengthInput.HasInterpolationPlaceholder())
            {
                ErrorMessage = string.Format(Resource.InvalidMinLengthInput, minValue);
            }

            _minValue = minValue;
        }

        public override ValidationResult GetValidationResult(string text)
        {
            if (string.IsNullOrWhiteSpace(text) && IsOptional)
            {
                return ValidationResultFactory.Success;
            }
            else if (string.IsNullOrWhiteSpace(text))
            {
                return ValidationResultFactory.Error(EmptyErrorMessage);
            }

            if (text.Length < _minValue)
            {
                return ValidationResultFactory.Error(ErrorMessage);
            }

            return ValidationResultFactory.Success;
        }
    }
}
