using FluentValidation;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale.Validators;

public class KernelConfigurationValidator : AbstractValidator<KernelConfigurationDto>
{
    public KernelConfigurationValidator()
    {
        RuleFor(x => x.KernelId).NotEmpty().Must(id => id > 0);

        RuleForEach(x => x.TagLengthValues).ChildRules(tlv =>
        {
            tlv.RuleFor(x => x.Tag).NotEmpty();
            tlv.RuleFor(x => x.Name).NotEmpty();
            tlv.RuleFor(x => x.Value).NotEmpty();
        });
    }
}
