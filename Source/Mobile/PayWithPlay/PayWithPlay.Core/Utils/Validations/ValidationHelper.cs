using PayWithPlay.Core.Interfaces;

namespace PayWithPlay.Core.Utils.Validations
{
    public class ValidationHelper
    {
        public static bool AreInputsValid(IList<ITextInputValidator> validations, bool setError)
        {
            if (validations == null || validations.Count == 0)
            {
                return true;
            }

            foreach (var textInput in validations)
            {
                var result = textInput.PerformValidation(setError, false);
                if (!result.IsValid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}