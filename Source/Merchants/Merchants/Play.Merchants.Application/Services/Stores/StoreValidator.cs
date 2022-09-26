using FluentValidation;

using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Application.Services;

public class StoreValidator : AbstractValidator<StoreDto>
{
    #region Constructor

    public StoreValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

        RuleFor(x => x.Address).NotEmpty();
    }

    #endregion
}