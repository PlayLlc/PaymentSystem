using FluentValidation;
using Play.MerchantPortal.Contracts.Messages.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale.Validators;

public class CombinationConfigurationValidator : AbstractValidator<CombinationConfigurationDto>
{
    public CombinationConfigurationValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
        RuleFor(x => x.TransactionType).Must(t => t > 0);
        RuleFor(x => x.ApplicationPriorityIndicator).Must(t => t > 0);
        RuleFor(x => x.ContactlessTransactionLimit).Must(t => t > 0);
        RuleFor(x => x.ContactlessFloorLimit).Must(t => t > 0);
        RuleFor(x => x.CvmRequiredLimit).Must(t => t > 0);
        RuleFor(x => x.KernelConfiguration).Must(t => t > 0);
        RuleFor(x => x.TerminalTransactionQualifiers).Must(t => t > 0);
    }
}
