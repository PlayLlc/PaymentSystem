using FluentValidation;
using Play.MerchantPortal.Contracts.Messages.PointOfSale;

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
