using FluentValidation;

using Play.Merchants.Contracts.DTO.PointOfSale;

namespace Play.Merchants.Application.Services;

public class ProximityCouplingDeviceConfigurationValidator : AbstractValidator<ProximityCouplingDeviceConfigurationDto>
{
    #region Constructor

    public ProximityCouplingDeviceConfigurationValidator()
    {
        RuleFor(x => (int) x.TimeoutValue).NotEmpty().GreaterThan(0);
    }

    #endregion
}