using FluentValidation;

using Play.Merchants.Contracts.Messages.PointOfSale;

namespace Play.Merchants.Application.Services.PointsOfSale.Validators;

public class ProximityCouplingDeviceConfigurationValidator : AbstractValidator<ProximityCouplingDeviceConfigurationDto>
{
    #region Constructor

    public ProximityCouplingDeviceConfigurationValidator()
    {
        RuleFor(x => (int) x.TimeoutValue).NotEmpty().GreaterThan(0);
    }

    #endregion
}