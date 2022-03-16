using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.DataElements;

public record CvmCodes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, CvmCodes> _ValueObjectMap;
    public static readonly CvmCodes Always;

    #endregion

    #region Constructor

    static CvmCodes()
    {
        Always = new CvmCodes(0);
        _ValueObjectMap = new Dictionary<byte, CvmCodes> {{Always, Always}}.ToImmutableSortedDictionary();
    }

    private CvmCodes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static bool TryGet(byte value, out CvmCodes? result) => _ValueObjectMap.TryGetValue(value, out result);
    public static bool IsValid(CvmConditionCode conditionCode) => _ValueObjectMap.ContainsKey((byte) conditionCode);

    #endregion

    #region Operator Overrides

    public static explicit operator CvmConditionCode(CvmCodes value) => new(value._Value);

    #endregion

    /*
     *
     *  public bool FailCardholderVerificationIfThisCvmIsUnsuccessful() => !_Value.IsBitSet(Bits.Seven);
    public bool ApplySucceedingCvRuleIfThisCvmIsUnsuccessful() => _Value.IsBitSet(Bits.Seven);
    public bool FailCvmProcessing() => _Value.GetMaskedValue(0b11000000) == 0;
    public bool PlaintextPinVerificationPerformedByIcc() => !_Value.IsBitSet(Bits.One);
    public bool EncipheredPinVerifiedOnline() => !_Value.IsBitSet(Bits.Two);
    public bool PlaintextPinVerificationPerformedByIccAndSignaturePaper() => !_Value.AreBitsSet(Bits.One, Bits.Two);
    public bool EncipheredPinVerificationPerformedByIcc() => !_Value.IsBitSet(Bits.Three);
    public bool EncipheredPinVerificationPerformedByIccAndSignaturePaper() => !_Value.AreBitsSet(Bits.One, Bits.Three);
    public bool SignaturePaper() => !_Value.AreBitsSet(0b00011110);
    public bool NoCvmRequired() => !_Value.AreBitsSet(0b00011111);
     */
}