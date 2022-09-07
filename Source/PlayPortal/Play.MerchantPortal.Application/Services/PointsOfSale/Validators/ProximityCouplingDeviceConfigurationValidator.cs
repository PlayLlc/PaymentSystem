using FluentValidation;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale.Validators;

public class ProximityCouplingDeviceConfigurationValidator : AbstractValidator<ProximityCouplingDeviceConfigurationDto>
{
    public ProximityCouplingDeviceConfigurationValidator()
    {
        RuleFor(x => (int)x.TimeoutValue)
            .NotEmpty()
            .GreaterThan(0);
    }
}
