namespace PayWithPlay.Core.Utils.Validations
{
    public class ValidationResultFactory
    {
        public static ValidationResult Success { get; } = new ValidationResult { IsValid = true };

        public static ValidationResult Error(string errorMessage) => new() { Message = errorMessage };
    }
}