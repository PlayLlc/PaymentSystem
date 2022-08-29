using FluentValidation;
using MerchantPortal.Application.DTO;

namespace MerchantPortal.Application.Services.Merchants;

public class MerchantValidator : AbstractValidator<MerchantDto>
{
    public MerchantValidator()
    {
        RuleFor(x => x.AcquirerId)
        .NotEmpty()
        .MaximumLength(15);

        RuleFor(x => x.MerchantIdentifier)
            .NotEmpty()
            .MaximumLength(15);

        RuleFor(x => (int)x.MerchantCategoryCode)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.StreetAddress)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();

        RuleFor(x => x.ZipCode)
            .NotEmpty();

        RuleFor(x => x.State)
            .NotEmpty();

        RuleFor(x => x.Country)
            .NotEmpty();
    }
}
