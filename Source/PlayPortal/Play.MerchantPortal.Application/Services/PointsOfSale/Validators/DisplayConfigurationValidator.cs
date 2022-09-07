using FluentValidation;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale.Validators;

public class DisplayConfigurationValidator : AbstractValidator<DisplayConfigurationDto>
{
    public DisplayConfigurationValidator()
    {
        RuleFor(x => x.MessageHoldTime).NotEmpty();

        RuleForEach(x => x.DisplayMessages)
            .ChildRules(displayMessageSet =>
            {
                displayMessageSet.RuleFor(x => x.LanguageCode).NotEmpty().Length(2);
                displayMessageSet.RuleFor(x => x.CountryCode).NotEmpty();
                displayMessageSet.RuleForEach(x => x.Messages).ChildRules(message =>
                {
                    message.RuleFor(x => x.MessageIdentifier).NotEmpty();
                    message.RuleFor(x => x.Message).NotEmpty();
                });
            });
    }
}
