using FluentValidation;

using Play.Merchants.Contracts.DTO.PointOfSale;

namespace Play.Merchants.Application.Services;

public class DisplayConfigurationValidator : AbstractValidator<DisplayConfigurationDto>
{
    #region Constructor

    public DisplayConfigurationValidator()
    {
        RuleFor(x => x.MessageHoldTime).NotEmpty();
        RuleFor(x => x.DisplayMessages).NotEmpty();
        RuleForEach(x => x.DisplayMessages)
            .ChildRules(displayMessageSet =>
            {
                displayMessageSet.RuleFor(x => x.LanguageCode).NotEmpty().Length(2);
                displayMessageSet.RuleFor(x => x.CountryCode).NotEmpty();
                displayMessageSet.RuleForEach(x => x.Messages)
                    .ChildRules(message =>
                    {
                        message.RuleFor(x => x.MessageIdentifier).NotEmpty();
                        message.RuleFor(x => x.Message).NotEmpty();
                    });
            });
    }

    #endregion
}