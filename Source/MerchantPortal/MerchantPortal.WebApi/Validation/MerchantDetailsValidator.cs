using FluentValidation;
using MerchantPortal.WebApi.Models;

namespace MerchantPortal.WebApi.Validation;

public class MerchantDetailsValidator : AbstractValidator<MerchantInsertRequest>
{
    public MerchantDetailsValidator()
    {
        RuleFor(x => x.AcquirerId)
            .NotNull()
            .NotEmpty()
            .MaximumLength(15);
    }
}
