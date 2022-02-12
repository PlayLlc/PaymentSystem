using Play.Core.Extensions;

public readonly struct CardholderVerificationRuleFormat
{
    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public CardholderVerificationRuleFormat(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public bool FailCardholderVerificationIfThisCvmIsUnsuccessful() => !_Value.IsBitSet(Bits.Seven);
    public bool ApplySucceedingCvRuleIfThisCvmIsUnsuccessful() => _Value.IsBitSet(Bits.Seven);
    public bool FailCvmProcessing() => _Value.GetMaskedValue(0b11000000) == 0;
    public bool PlaintextPinVerificationPerformedByIcc() => !_Value.IsBitSet(Bits.One);
    public bool EncipheredPinVerifiedOnline() => !_Value.IsBitSet(Bits.Two);
    public bool PlaintextPinVerificationPerformedByIccAndSignaturePaper() => !_Value.AreBitsSet(Bits.One, Bits.Two);
    public bool EncipheredPinVerificationPerformedByIcc() => !_Value.IsBitSet(Bits.Three);
    public bool EncipheredPinVerificationPerformedByIccAndSignaturePaper() => !_Value.AreBitsSet(Bits.One, Bits.Three);
    public bool SignaturePaper() => !_Value.AreBitsSet(0b00011110);
    public bool NoCvmRequired() => !_Value.AreBitsSet(0b00011111);

    #endregion

    #region Operator Overrides

    public static explicit operator byte(CardholderVerificationRuleFormat value) => value._Value;

    #endregion
}