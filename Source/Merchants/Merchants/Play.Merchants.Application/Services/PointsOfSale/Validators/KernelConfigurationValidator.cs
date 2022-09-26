using FluentValidation;

using Play.Merchants.Contracts.Messages.PointOfSale;

namespace Play.Merchants.Application.Services.PointsOfSale.Validators;

public class KernelConfigurationValidator : AbstractValidator<KernelConfigurationDto>
{
    #region Constructor

    public KernelConfigurationValidator()
    {
        RuleFor(x => x.KernelId).NotEmpty().Must(id => id > 0);
        RuleFor(x => x.TagLengthValues).NotEmpty();
        RuleForEach(x => x.TagLengthValues)
            .ChildRules(tlv =>
            {
                tlv.RuleFor(x => x.Tag).NotEmpty();
                tlv.RuleFor(x => x.Name).NotEmpty();
                tlv.RuleFor(x => x.Value).NotEmpty();
            });
    }

    #endregion
}