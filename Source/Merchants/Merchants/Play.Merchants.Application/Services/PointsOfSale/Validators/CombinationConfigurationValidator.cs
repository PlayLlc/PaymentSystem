using FluentValidation;

using Play.Merchants.Contracts.DTO;

namespace Play.Merchants.Application.Services;

public class CombinationConfigurationValidator : AbstractValidator<CombinationConfigurationDto>
{
    #region Constructor

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

    #endregion
}