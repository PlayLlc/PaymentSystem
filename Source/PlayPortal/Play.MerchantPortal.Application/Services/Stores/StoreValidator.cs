using FluentValidation;
using Play.MerchantPortal.Contracts.DTO;

namespace Play.MerchantPortal.Application.Services.Stores;

internal class StoreValidator : AbstractValidator<StoreDto>
{
    public StoreValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Address)
            .NotEmpty();
    }
}
