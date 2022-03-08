namespace Play.Emv.DataElements.Emv;

public readonly struct CardholderVerificationRules
{
    #region Instance Values

    private readonly CardholderVerificationRuleFormat _CardholderVerificationRuleFormat;
    private readonly CvmConditionCodes _CvmConditionCodes;

    #endregion

    #region Constructor

    public CardholderVerificationRules(ReadOnlySpan<byte> value)
    {
        if (value.Length != 2)
            throw new ArgumentOutOfRangeException(nameof(value));

        _CardholderVerificationRuleFormat = new CardholderVerificationRuleFormat(value[0]);
        _CvmConditionCodes = new CvmConditionCodes(value[1]);
    }

    #endregion

    #region Instance Members

    public bool FailCardholderVerificationIfThisCvmIsUnsuccessful() =>
        _CardholderVerificationRuleFormat.FailCardholderVerificationIfThisCvmIsUnsuccessful();

    public bool ApplySucceedingCvRuleIfThisCvmIsUnsuccessful() =>
        _CardholderVerificationRuleFormat.ApplySucceedingCvRuleIfThisCvmIsUnsuccessful();

    public bool FailCvmProcessing() => _CardholderVerificationRuleFormat.FailCvmProcessing();
    public bool PlaintextPinVerificationPerformedByIcc() => _CardholderVerificationRuleFormat.PlaintextPinVerificationPerformedByIcc();
    public bool EncipheredPinVerifiedOnline() => _CardholderVerificationRuleFormat.EncipheredPinVerifiedOnline();

    public bool PlaintextPinVerificationPerformedByIccAndSignaturePaper() =>
        _CardholderVerificationRuleFormat.PlaintextPinVerificationPerformedByIccAndSignaturePaper();

    public bool EncipheredPinVerificationPerformedByIcc() => _CardholderVerificationRuleFormat.EncipheredPinVerificationPerformedByIcc();

    public bool EncipheredPinVerificationPerformedByIccAndSignaturePaper() =>
        _CardholderVerificationRuleFormat.EncipheredPinVerificationPerformedByIccAndSignaturePaper();

    public bool SignaturePaper() => _CardholderVerificationRuleFormat.SignaturePaper();
    public bool NoCvmRequired() => _CardholderVerificationRuleFormat.NoCvmRequired();
    public bool Always() => _CvmConditionCodes.Always();
    public bool IfUnattendedCash() => _CvmConditionCodes.IfUnattendedCash();

    public bool IfNotUnattendedCashAndNotManualCashAndNotPurchaseWithCashback() =>
        _CvmConditionCodes.IfNotUnattendedCashAndNotManualCashAndNotPurchaseWithCashback();

    public bool IfTerminalSupportsCvm() => _CvmConditionCodes.IfTerminalSupportsCvm();
    public bool IfManualCash() => _CvmConditionCodes.IfManualCash();
    public bool IfPurchaseWithCashback() => _CvmConditionCodes.IfPurchaseWithCashback();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsUnderXValue() =>
        _CvmConditionCodes.IfTransactionIsInTheApplicationCurrencyAndIsUnderXValue();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsOverXValue() =>
        _CvmConditionCodes.IfTransactionIsInTheApplicationCurrencyAndIsOverXValue();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsUnderYValue() =>
        _CvmConditionCodes.IfTransactionIsInTheApplicationCurrencyAndIsUnderYValue();

    public bool IfTransactionIsInTheApplicationCurrencyAndIsOverYValue() =>
        _CvmConditionCodes.IfTransactionIsInTheApplicationCurrencyAndIsOverYValue();

    #endregion
}