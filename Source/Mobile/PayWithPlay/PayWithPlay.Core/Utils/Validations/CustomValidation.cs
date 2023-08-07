namespace PayWithPlay.Core.Utils.Validations
{
    public class CustomValidation : BaseValidation
    {
        private readonly Func<string, ValidationResult>? _validationAction;

        public CustomValidation(Func<string, ValidationResult>? validationAction) : base("")
        {
            _validationAction = validationAction;
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

            if (_validationAction == null) 
            {
                return ValidationResultFactory.Success;
            }

            return _validationAction.Invoke(value);
        }
    }
}