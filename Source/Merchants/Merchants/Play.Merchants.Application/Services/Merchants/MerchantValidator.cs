using FluentValidation;

using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Application.Services.Merchants;

public class MerchantValidator : AbstractValidator<MerchantDto>
{
    #region Static Metadata

    private const string _AlphabeticOnlyRegex = @"^[a-zA-Z]+$";
    private const string _DigitsOnlyRegex = @"^[0-9]+$";

    #endregion

    #region Constructor

    public MerchantValidator()
    {
        RuleFor(x => x.AcquirerId).NotEmpty().MaximumLength(15);

        RuleFor(x => x.MerchantIdentifier).NotEmpty().MaximumLength(15);

        RuleFor(x => (int) x.MerchantCategoryCode).GreaterThan(0);

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.StreetAddress).NotEmpty();

        RuleFor(x => x.City).NotEmpty().MaximumLength(50).Matches(_AlphabeticOnlyRegex);

        RuleFor(x => x.ZipCode).NotEmpty().Length(0, 5).Matches(_DigitsOnlyRegex);

        RuleFor(x => x.State).NotEmpty().MaximumLength(50).Matches(_AlphabeticOnlyRegex);

        RuleFor(x => x.Country).NotEmpty().MaximumLength(50).Matches(_AlphabeticOnlyRegex);
    }

    #endregion
}