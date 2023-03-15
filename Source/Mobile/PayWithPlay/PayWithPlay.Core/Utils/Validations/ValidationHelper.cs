using PayWithPlay.Core.Interfaces;

namespace PayWithPlay.Core.Utils.Validations
{
    public class ValidationHelper
    {
        public static bool AreInputsValid(IList<ITextInputValidator> validators, bool setError)
        {
            if (validators == null || validators.Count == 0)
            {
                return true;
            }

            foreach (var validator in validators)
            {
                if (!IsInputValid(validator, setError))
                {
                    return false;
                }
            } 

            return true;
        }

        public static bool IsInputValid(ITextInputValidator validator, bool setError)
        {
            if (validator == null)
            {
                return true;
            }

            var result = validator.PerformValidation(setError, false);
            return result.IsValid;
        }
    }
}