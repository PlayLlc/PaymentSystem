using FluentValidation;
using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Application.Services.Merchants;

internal class MerchantValidator : AbstractValidator<MerchantDto>
{
    private const string _AlphabeticOnlyRegex = @"^[a-zA-Z]+$";
    private const string _DigitsOnlyRegex = @"^[0-9]+$";

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
            .NotEmpty()
            .MaximumLength(50)
            .Matches(_AlphabeticOnlyRegex);

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Length(0,5)
            .Matches(_DigitsOnlyRegex);

        RuleFor(x => x.State)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(_AlphabeticOnlyRegex);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(_AlphabeticOnlyRegex);
    }
}
