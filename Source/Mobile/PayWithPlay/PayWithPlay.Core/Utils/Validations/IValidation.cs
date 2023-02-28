
namespace PayWithPlay.Core.Utils.Validations
{
    public interface IValidation
    {
        bool IsOptional { get; set; }

        ValidationResult GetValidationResult(string value);
    }
}
