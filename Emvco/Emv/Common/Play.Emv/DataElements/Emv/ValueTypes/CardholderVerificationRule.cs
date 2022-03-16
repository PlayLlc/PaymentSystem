using System;

using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record CardholderVerificationRule
{
    #region Instance Values

    private readonly CvmCode _CvmCode;
    private readonly CvmConditionCode _CvmConditionCode;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    public CardholderVerificationRule(ReadOnlySpan<byte> value)
    {
        if (value.Length != 2)
            throw new DataElementParsingException(nameof(value));

        _CvmCode = new CvmCode(value[0]);
        _CvmConditionCode = new CvmConditionCode(value[1]);
    }

    #endregion

    #region Instance Members

    public bool FailCardholderVerificationIfThisCvmIsUnsuccessful() => _CvmCode.FailCardholderVerificationIfThisCvmIsUnsuccessful();
    public bool ApplySucceedingCvRuleIfThisCvmIsUnsuccessful() => _CvmCode.ApplySucceedingCvRuleIfThisCvmIsUnsuccessful();
    public bool FailCvmProcessing() => _CvmCode.FailCvmProcessing();
    public bool PlaintextPinVerificationPerformedByIcc() => _CvmCode.PlaintextPinVerificationPerformedByIcc();
    public bool EncipheredPinVerifiedOnline() => _CvmCode.EncipheredPinVerifiedOnline();

    public bool PlaintextPinVerificationPerformedByIccAndSignaturePaper() =>
        _CvmCode.PlaintextPinVerificationPerformedByIccAndSignaturePaper();

    public bool EncipheredPinVerificationPerformedByIcc() => _CvmCode.EncipheredPinVerificationPerformedByIcc();

    public bool EncipheredPinVerificationPerformedByIccAndSignaturePaper() =>
        _CvmCode.EncipheredPinVerificationPerformedByIccAndSignaturePaper();

    public bool SignaturePaper() => _CvmCode.SignaturePaper();
    public bool NoCvmRequired() => _CvmCode.NoCvmRequired();
    public bool Always() => _CvmConditionCode.Always();
    public bool IfUnattendedCash() => _CvmConditionCode.IfUnattendedCash();

    public bool IfNotUnattendedCashAndNotManualCashAndNotPurchaseWithCashback() =>
        _CvmConditionCode.IfNotUnattendedCashAndNotManualCashAndNotPurchaseWithCashback();

    public bool IfTerminalSupportsCvm() => _CvmConditionCode.IfTerminalSupportsCvm();
    public bool IfManualCash() => _CvmConditionCode.IfManualCash();
    public bool IfPurchaseWithCashback() => _CvmConditionCode.IfPurchaseWithCashback();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsUnderXValue() =>
        _CvmConditionCode.IfTransactionIsInTheApplicationCurrencyAndIsUnderXValue();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsOverXValue() =>
        _CvmConditionCode.IfTransactionIsInTheApplicationCurrencyAndIsOverXValue();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsUnderYValue() =>
        _CvmConditionCode.IfTransactionIsInTheApplicationCurrencyAndIsUnderYValue();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsOverYValue() =>
        _CvmConditionCode.IfTransactionIsInTheApplicationCurrencyAndIsOverYValue();

    #endregion
}