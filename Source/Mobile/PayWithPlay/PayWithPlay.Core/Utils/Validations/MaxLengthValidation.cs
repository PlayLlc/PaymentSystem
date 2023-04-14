using PayWithPlay.Core.Extensions;
using PayWithPlay.Core.Resources;

namespace PayWithPlay.Core.Utils.Validations
{
    public class MaxLengthValidation : BaseValidation
    {
        private readonly int _maxValue;

        public MaxLengthValidation(int maxValue = int.MaxValue, string errorMessage = "") : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage) && Resource.InvalidMaxLengthInput.HasInterpolationPlaceholder())
            {
                ErrorMessage = string.Format(Resource.InvalidMaxLengthInput, maxValue);
            }

            _maxValue = maxValue;
        }

        public override ValidationResult GetValidationResult(string value)
        {
            if (string.IsNullOrWhiteSpace(value) && IsOptional)
            {
                return ValidationResultFactory.Success;
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                return ValidationResultFactory.Error(EmptyErrorMessage);
            }

            if (value.Length > _maxValue)
            {
                return ValidationResultFactory.Error(ErrorMessage);
            }

            return ValidationResultFactory.Success;
        }
    }
}
