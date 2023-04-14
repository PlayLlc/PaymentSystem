using System.Text.RegularExpressions;

namespace PayWithPlay.Core.Utils.Validations
{
    public class RegexValidation : BaseValidation
    {
        private string _regex;

        public RegexValidation(string regex, string errorMessage) : base(errorMessage)
        {
            _regex = regex;
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

            var result = Regex.IsMatch(text, _regex);
            if (!result)
            {
                return ValidationResultFactory.Error(ErrorMessage);
            }

            return ValidationResultFactory.Success;
        }
    }
}