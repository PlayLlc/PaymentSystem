﻿using FluentValidation;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale.Validators;

public class TerminalConfigurationValidator : AbstractValidator<TerminalConfigurationDto>
{
    public TerminalConfigurationValidator()
    {
        RuleFor(x => x.TerminalIdentification)
            .NotEmpty();
        RuleFor(x => x.MerchantIdentifier)
            .NotEmpty();
        RuleFor(x => x.MerchantCategoryCode)
            .NotEmpty();
        RuleFor(x => x.MerchantNameAndLocation)
            .NotEmpty();
        RuleFor(x => x.AcquirerIdentifier)
            .NotEmpty();
        RuleFor(x => x.InterfaceDeviceSerialNumber)
            .NotEmpty();
        RuleFor(x => x.TerminalType)
            .NotEmpty();
        RuleFor(x => x.TerminalCapabilities)
            .NotEmpty();
        RuleFor(x => x.AdditionalTerminalCapabilities)
            .NotEmpty();
        RuleFor(x => x.TerminalCountryCode)
            .NotEmpty();
        RuleFor(x => x.LanguagePreference)
            .NotEmpty();
        RuleFor(x => x.TransactionCurrencyCode)
            .NotEmpty();
        RuleFor(x => x.TransactionCurrencyExponent)
            .NotEmpty();
        RuleFor(x => x.TransactionReferenceCurrencyCode)
            .NotEmpty();
        RuleFor(x => x.TransactionReferenceCurrencyExponent)
            .NotEmpty();
        RuleFor(x => x.TerminalFloorLimit)
            .NotEmpty();
        RuleFor(x => x.TerminalRiskManagementData)
            .NotEmpty();
        RuleFor(x => x.DataStorageRequestedOperatorId)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.BiasedRandomSelectionProbability)
            .NotEmpty();
        RuleFor(x => x.BiasedRandomSelectionTargetProbability)
            .NotEmpty();
        RuleFor(x => x.ThresholdValueForBiasedRandomSelection)
            .NotEmpty();
        RuleFor(x => x.MaxNumberOfTornTransactionLogRecords)
            .NotEmpty();
        RuleFor(x => x.MaxLifetimeOfTornTransactionLogRecords)
            .NotEmpty();
        RuleFor(x => x.SequenceConfiguration)
            .NotNull();
    }
}
